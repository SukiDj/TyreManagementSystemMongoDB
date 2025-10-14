import { makeAutoObservable } from "mobx";
import agent from "../../api/agent";
import { ProductionData } from "../models/Production";
import { SalesData } from "../models/Sale";

export default class BusinessUnitStore {
    productionData: ProductionData[] = [];
    salesData: SalesData[] = [];
    loading = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadProductionData = async () => {
        this.loading = true;
        try {
            const data = await agent.BusinessUnit.getProductionData();
            this.productionData = data;
            this.loading = false;
        } catch (error) {
            console.error("Failed to load production data:", error);
            this.loading = false;
        }
    }

    loadSalesData = async () => {
        this.loading = true;
        try {
            const data = await agent.BusinessUnit.getSalesData();
            this.salesData = data;
            this.loading = false;
        } catch (error) {
            console.error("Failed to load sales data:", error);
            this.loading = false;
        }
    }
}
