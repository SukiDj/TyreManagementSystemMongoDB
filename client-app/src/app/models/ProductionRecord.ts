export interface ProductionRecord {
    id?: string;
    tyreCode: string;
    quantityProduced: number;
    productionDate: Date | null;
    shift: number;
    machineNumber: number;
    operatorId: string;
    machineName?: string;
    operatorName?: string;
    tyreType?: string;
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
    machineName?: string = undefined;
    operatorName?: string = undefined;
    tyreType?: string = undefined;

    constructor(record?: RecordFromValues) {
        if (record) {
            this.id = record.id;
            this.tyreId = record.tyreId;
            this.quantityProduced = record.quantityProduced;
            this.shift = record.shift;
            this.machineId = record.machineId;
            this.machineName = record.machineName;
            this.operatorName = record.operatorName;
            this.tyreType = record.tyreType;
        }
    }
}
