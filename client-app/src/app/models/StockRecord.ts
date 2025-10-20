export interface StockRecord {
    tyreCode: string;
    stockBalance: number;
    balanceDate: Date;
    tyreType?: string;
}

export class StockRecord implements StockRecord {
    constructor(init?: StockRecordFormValues) {
        Object.assign(this, init);
    }
}

export class StockRecordFormValues {
    tyreCode: string = '';
    stockBalance: number = 0;
    balanceDate: Date = new Date();
    tyreType?: string = undefined;

    constructor(stockRecord?: StockRecordFormValues) {
        if (stockRecord) {
            this.tyreCode = stockRecord.tyreCode;
            this.stockBalance = stockRecord.stockBalance;
            this.balanceDate = stockRecord.balanceDate;
            this.tyreType = stockRecord.tyreType;
        }
    }
}
