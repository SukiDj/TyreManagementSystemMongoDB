export interface StockRecord {
    tyreCode: string;
    stockBalance: number;
    balanceDate: Date;
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

    constructor(stockRecord?: StockRecordFormValues) {
        if (stockRecord) {
            this.tyreCode = stockRecord.tyreCode;
            this.stockBalance = stockRecord.stockBalance;
            this.balanceDate = stockRecord.balanceDate;
        }
    }
}
