export interface ProductionRecord {
    id?: string;
    tyreCode: string;
    quantityProduced: number;
    productionDate: Date | null;
    shift: number;
    machineNumber: number;
    operatorId: string;
}

export class ProductionRecord implements ProductionRecord {
    constructor(init?: RecordFromValues) {
        Object.assign(this, init);
    }
}

export class RecordFromValues {
    id?: string = undefined;
    tyreId: string = '';
    machineId: string = '';
    shift: number = 0;
    quantityProduced: number = 0;

    constructor(record?: RecordFromValues) {
        if (record) {
            this.id = record.id;
            this.tyreId = record.tyreId;
            this.quantityProduced = record.quantityProduced;
            this.shift = record.shift;
            this.machineId = record.machineId;
        }
    }
}
