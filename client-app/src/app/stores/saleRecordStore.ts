import { makeAutoObservable, runInAction } from "mobx";
import { SaleRecord, SaleRecordFromValues } from "../models/SaleRecord";
import agent from "../../api/agent";
import { format } from "date-fns";
import { Pagination, PagingParams } from "../models/Pagination";
import { store } from "./store";

export default class RecordStore {
    recordRegistry = new Map<string, SaleRecord>();
    selectedRecord: SaleRecord | undefined = undefined;
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

    createRecord = async (record: SaleRecordFromValues) => {
        this.isSubmitting = true;
        try {
            

            await agent.QualitySupervisor.registerSale(record);
            const newRecord = new SaleRecord(record);
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

    updateRecord = async (id: string | undefined, record: SaleRecordFromValues) => {
        try {
            const saleUpdate = {
                id: id,
                pricePerUnit: record.pricePerUnit,
                quantitySold: record.quantitySold,
                unitOfMeasure: record.unitOfMeasure,
                tyreId: record.tyreId,
                clientId: record.clientId,
                productionOrderId: record.productionOrderId,
                targetMarket: record.targetMarket
            };
            console.log(id, "aj sad")
            await agent.Records.updateSale(id!, saleUpdate);

            runInAction(() => {
                if (id!) {
                    const updatedRecord = { ...this.getRecord(id!), ...record };
                    this.recordRegistry.set(id!, updatedRecord as SaleRecord);
                    this.selectedRecord = updatedRecord as SaleRecord;
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
                const date = format(record.saleDate!, 'dd MMM yyyy'); 
                records[date] = records[date] ? [...records[date], record] : [record];
                return records;
            }, {} as { [key: string]: SaleRecord[] }) 
        );
    }

    private setRecord = (record:SaleRecord)=>{
        const user = store.userStore.user;
        if(user){
            
        }
        record.saleDate = new Date(record.saleDate!);
        this.recordRegistry.set(record.id!,record);
    }

    

    loadSaleRecords = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Records.getAllSaleHistory();
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
    
    deleteSaleRecord = async (id: string) => {
        try {
          await agent.Records.deleteSaleRecord(id); // API poziv za brisanje
          runInAction(() => {
            this.recordRegistry.delete(id);
          });
        } catch (error) {
          console.log(error);
        }
    };
}
