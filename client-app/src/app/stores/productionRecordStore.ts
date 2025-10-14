import { makeAutoObservable, runInAction } from "mobx";
import { ProductionRecord, RecordFromValues } from "../models/ProductionRecord";
import agent from "../../api/agent";
import { format } from "date-fns";
import { Pagination, PagingParams } from "../models/Pagination";
import { store } from "./store";
import { date } from "yup";

export default class RecordStore {
    recordRegistry = new Map<string, ProductionRecord>();
    selectedRecord: ProductionRecord | undefined = undefined;
    isSubmitting = false;
    loadingInitial = false;
    pagingParams = new PagingParams();
    pagination: Pagination | null = null;
    loadingNext = false;


    constructor() {
        makeAutoObservable(this);
    }

    setLoadingInitial = (state:boolean)=>{
        
        this.loadingInitial = state;
    }

    setPagingParams=(pagingParams:PagingParams)=>{
        this.pagingParams = pagingParams;
    }

    setPagination = (pagination:Pagination) =>{
        this.pagination = pagination
    }

    setLoadingNext =(state:boolean)=>{
        this.loadingNext = state;  
    }

    createRecord = async (record: RecordFromValues) => {
        this.isSubmitting = true;
        try {
            

            await agent.ProductionOperator.registerProduction(record);
            const newRecord = new ProductionRecord(record);
            console.log(newRecord);
            runInAction(() => {
                this.recordRegistry.set(newRecord.id!, newRecord);
                this.selectedRecord = newRecord;
                this.isSubmitting = false;
            });
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.isSubmitting = false;
            });
        }
    };

    updateRecord = async (record: RecordFromValues) => {
        try {
            const productionUpdate = {
                shift: record.shift,
                quantityProduced: record.quantityProduced
            };

            await agent.Records.updateProduction(record.id!, productionUpdate);

            runInAction(() => {
                if (record.id) {
                    const updatedRecord = { ...this.getRecord(record.id), ...record };
                    this.recordRegistry.set(record.id, updatedRecord as ProductionRecord);
                    this.selectedRecord = updatedRecord as ProductionRecord;
                }
            });
        } catch (error) {
            console.log(error);
        }
    };

    private getRecord = (id: string) => {
        return this.recordRegistry.get(id);
    };

    get groupedRecords() {
        return Object.entries(
            Array.from(this.recordRegistry.values()).reduce((records, record) => {
                const date = format(record.productionDate!, 'dd MMM yyyy'); 
                records[date] = records[date] ? [...records[date], record] : [record];
                return records;
            }, {} as { [key: string]: ProductionRecord[] }) 
        );
    }

    private setRecord = (record:ProductionRecord)=>{
        const user = store.userStore.user;
        if(user){
            
        }
        record.productionDate = new Date(record.productionDate!);
        this.recordRegistry.set(record.id!,record);
    }

    

    loadProductionRecords = async (userId: string) => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Records.getProductionHistory(userId);
            console.log(result);
            result.data.forEach(record => {
                this.setRecord(record); 
            });
            //this.setPagination(result.pagination); // Set pagination if applicable
            this.setLoadingInitial(false); // Set loading state to false after loading is complete
        } catch (error) {
            console.log(error); // Log any errors
            this.setLoadingInitial(false); // Ensure the loading state is reset even if an error occurs
        }
    }

    loadAllProductionRecords = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Records.getAllProductionHistory();
            console.log(result);
            result.data.forEach(record => {
                this.setRecord(record); 
            });
            //this.setPagination(result.pagination); // Set pagination if applicable
            this.setLoadingInitial(false); // Set loading state to false after loading is complete
        } catch (error) {
            console.log(error); // Log any errors
            this.setLoadingInitial(false); // Ensure the loading state is reset even if an error occurs
        }
    }

    loadProductionByDay = async (date: Date) => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.BusinessUnit.productionByDay(date);
            runInAction(() => {
                this.recordRegistry.clear();
                result.forEach((record: ProductionRecord) => {
                    this.setRecord(record);
                });
                this.setLoadingInitial(false);
            });
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }
    

    loadProductionByShift = async (shift: number) => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.BusinessUnit.productionByShift(shift);
            runInAction(() => {
                this.recordRegistry.clear();
                result.forEach((record: ProductionRecord) => {
                    this.setRecord(record);
                });
                this.setLoadingInitial(false);
            });
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }
    
    
    loadProductionByMachine = async (machineId: string) => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.BusinessUnit.productionByMachine(machineId);
            runInAction(() => {
                this.recordRegistry.clear();
                result.forEach((record: ProductionRecord) => {
                    this.setRecord(record);
                });
                this.setLoadingInitial(false);
            });
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }
    

    loadProductionByOperator = async (operatorId: string) => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.BusinessUnit.productionByOperator(operatorId);
            runInAction(() => {
                this.recordRegistry.clear();
                result.forEach((record: ProductionRecord) => {
                    this.setRecord(record);
                });
                this.setLoadingInitial(false);
            });
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }
    
    

}
