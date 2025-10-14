export interface SaleRecord {
    id?: string;
    tyreCode: string;
    clientId: string;
    productionOrderId: string;
    unitOfMeasure: string;
    pricePerUnit: number;
    quantitySold: number;
    saleDate: Date | null;
    targetMarket: string;
}

export class SaleRecord implements SaleRecord {
    constructor(init?: SaleRecordFromValues) {
        Object.assign(this, init);
    }
}

export class SaleRecordFromValues {
    id?: string = undefined;
    tyreId: string = '';
    clientId: string = '';
    productionOrderId: string = '';
    unitOfMeasure: string = '';
    pricePerUnit: number = 0;
    quantitySold: number = 0;
    targetMarket: string = '';

    constructor(record?: SaleRecordFromValues) {
        if (record) {
            this.id = record.id;
            this.tyreId = record.tyreId;
            this.clientId = record.clientId;
            this.productionOrderId = record.productionOrderId;
            this.quantitySold = record.quantitySold;
            this.unitOfMeasure = record.unitOfMeasure;
            this.pricePerUnit = record.pricePerUnit;
            this.targetMarket = record.targetMarket;
        }
    }
}
