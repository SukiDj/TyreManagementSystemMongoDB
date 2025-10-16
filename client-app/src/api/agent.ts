import axios, { AxiosError, AxiosResponse } from 'axios';
import { store } from '../app/stores/store';
import { PaginatedResult } from "../app/models/Pagination";
import { router } from '../app/router/Routes';
import { toast } from 'react-toastify';
import { RegisterUserFormValues} from '../app/models/User';
import { User, UserFormValues } from '../app/models/User';
import { ProductionData } from '../app/models/Production';
import { SalesData } from '../app/models/Sale';
import { ProductionRecord, RecordFromValues } from '../app/models/ProductionRecord';
import { Tyre } from '../app/models/tyre';
import { Machine } from '../app/models/Machine';
import { SaleRecord, SaleRecordFromValues } from '../app/models/SaleRecord';
import { Client } from '../app/models/Client';
import { StockRecord } from '../app/models/StockRecord';
import { ActionLog } from '../app/models/ActionLog';

const sleep =(delay: number) =>{
    return new Promise((resolve)=>{
        setTimeout(resolve,delay)
    })
}

axios.defaults.baseURL=import.meta.env.VITE_API_URL;

axios.interceptors.request.use(config => {
    const token= store.commonStore.token;
    if(token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response =>{
        if (import.meta.env.DEV) await sleep(1000);// NOVO
        const pagination = response.headers['pagination'];
        if(pagination){
            response.data = new PaginatedResult(response.data,JSON.parse(pagination));
            return response as AxiosResponse<PaginatedResult<any>>
        }
        return response;

}, (error: AxiosError) =>{
    const {data,status, config, headers} = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if(config.method === 'get' && Object.prototype.hasOwnProperty.call(data.errors,'id')){
                router.navigate('/not-found');
            }
            if(data.errors){
                const modalStateErrors =[];
                for(const key in data.errors){
                    if(data.errors[key]){
                        modalStateErrors.push(data.errors[key])
                    }
                }
                throw modalStateErrors.flat();
            } else{
                toast.error(data);
                //console.log("odavde");
            }
            toast.error('bad request');
            //console.log("odavde");
            break;

        case 401:
            if(status === 401 && headers['www-authenticate']?.startsWith('Bearer error="invalid_token'))
            {
                store.userStore.loguot();
                toast.error('Sesija je istekla - molimo Vas da se ulogujete ponovo.');
            } else{
                toast.error('unauthorised');
            }
            break;
            
        case 403:
            toast.error('forbidden');
            break;
            
        case 404:
            router.navigate('/not found');
            break;
            
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;
    }
    return Promise.reject(error);
})


const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url:string) => axios.get<T>(url).then(responseBody),
    post: <T>(url:string, body:{}) => axios.post<T>(url, body).then(responseBody),
    postFormData: <T>(url: string, formData: FormData) => axios.post<T>(url, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    }).then(responseBody),
    put: <T>(url:string,body:{}) => axios.put<T>(url,body).then(responseBody),
    del: <T>(url:string)=> axios.delete<T>(url).then(responseBody)
}

const Records = {
    registerProduction: (productionData: { shift: number, quantityProduced: number, tyreId: string, machineId: string }) =>
        requests.post<void>('/ProductionOperator/registerProduction', productionData),

    registerSale: (saleData: { 
        pricePerUnit: number; 
        clientId: string; 
        quantitySold: number; 
        tyreId: string; 
        productionOrderId: string; 
        unitOfMeasure: string; 
        targetMarket: string; 
    }) =>
        requests.post<void>('/QualitySupervisor/registerTyreSale', saleData),
        
    updateProduction: (id: string, productionUpdate: { shift: number, quantityProduced: number}) =>
        axios.put<void>(`/QualitySupervisor/updateProduction/${id}`, {
            params: {
                shift: productionUpdate.shift,
                quantityProduced: productionUpdate.quantityProduced
            }
        }),

        updateSale: (id: string, saleUpdate: { pricePerUnit: number, clientId: string, quantitySold: number, tyreId: string }) =>
            axios.put<void>(`/QualitySupervisor/updateSale/${id}`, null, {
                params: {
                    tyreId: saleUpdate.tyreId,
                    clientId: saleUpdate.clientId,
                    quantitySold: saleUpdate.quantitySold,
                    pricePerUnit: saleUpdate.pricePerUnit
                }
            }),

    deleteSaleRecord: (id: string) => requests.del(`/QualitySupervisor/deleteSale/${id}`),
        
    getProductionHistory: (operatorId: string) => axios.get<ProductionRecord[]>(`/ProductionOperator/history`),

    getAllProductionHistory: () => axios.get<ProductionRecord[]>(`/QualitySupervisor/productionHistory`),

    getProductionHistoryRq: (operatorId: string) => requests.get<ProductionRecord[]>(`/ProductionOperator/history`),

    getAllProductionHistoryRq: () => requests.get<ProductionRecord[]>(`/QualitySupervisor/productionHistory`),

    getAllSaleHistory: () => axios.get<SaleRecord[]>(`/QualitySupervisor/saleHistory`)
}

const Account = {
    current: () => requests.get<User>('/Account'),
    login: (user:UserFormValues) => requests.post<User>('/Account/login', user),
    refreshToken: () => requests.post<User>('/Account/refreshToken/', {}),
    register: (user: RegisterUserFormValues) => requests.post<User>('/Account/register', user),
    getOperators: () => requests.get<User[]>('/Account/operators')
}

const BusinessUnit = {
    getProductionData: (): Promise<ProductionData[]> => requests.get(`/businessunit/getProductions`),
    getSalesData: (): Promise<SalesData[]> => requests.get(`/businessunit/getSales`),
    productionByDay: (date: Date) => requests.get<ProductionRecord[]>(`/businessunit/productionByDay?date=${date.toISOString()}`),
    productionByShift: (shift: number) => requests.get<ProductionRecord[]>(`/businessunit/productionByShift?shift=${shift}`),
    productionByMachine: (machineId: string) => requests.get<ProductionRecord[]>(`/businessunit/productionByMachine?machineId=${machineId}`),
    productionByOperator: (operatorId: string) => requests.get<ProductionRecord[]>(`/businessunit/productionByOperator?operatorId=${operatorId}`),
    stockBalance: (date: Date) => requests.get<StockRecord[]>(`/businessunit/stockBalance?date=${date.toISOString()}`)
};

const ProductionOperator ={
    registerProduction: (production:RecordFromValues) => requests.post<void>(`/ProductionOperator/registerProduction`, production)
}

const QualitySupervisor ={
    registerSale: (sale:SaleRecordFromValues) => requests.post<void>(`/QualitySupervisor/registerTyreSale`, sale)
}

const Tyres = {
    getTyres: () => requests.get<Tyre[]>('/Tyre/GetTyres')
}

const Machines = {
    getMachines: () => requests.get<Machine[]>('/Machine/GetMachines')
}

const Clients = {
    getClients: () => requests.get<Client[]>('/Client/GetClients')
}

const StockRecords = {
    getBalance: (date: Date) => requests.get<StockRecord[]>(`/stockbalance?date=${date.toISOString()}`),
};

const Logs = {
    list: () => requests.get<ActionLog[]>('/ActionLog/GetActions'), // Poziv GET /api/log za povlačenje logova
};

const agent = {
    Account,
    Records,
    BusinessUnit,
    Tyres,
    Machines,
    Clients,
    StockRecords,
    Logs,
    ProductionOperator,
    QualitySupervisor
};

export default agent;