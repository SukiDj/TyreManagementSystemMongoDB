import { makeAutoObservable, runInAction } from "mobx";
import { StockRecord } from "../models/StockRecord";
import agent from "../../api/agent";

export default class StockRecordStore {
    stockBalance: StockRecord[] = [];
    loadingInitial = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadStockBalance = async (date: Date) => {
        this.loadingInitial = true;
        try {
            const stockBalance = await agent.BusinessUnit.stockBalance(date);
            runInAction(() => {
                this.stockBalance = stockBalance;
                this.loadingInitial = false;
            });
        } catch (error) {
            runInAction(() => {
                this.loadingInitial = false;
            });
            console.error(error);
        }
    };
}
