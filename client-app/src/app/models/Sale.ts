export interface SalesData {
    id: string;
    saleDate: string; // ili Date
    quantitySold: number;
    pricePerUnit: number;
    unitOfMeasure: string;
    targetMarket: string;
    //tyre: Tyre; // veza sa entitetom Tyre
    //client: Client; // veza sa entitetom Client
    //production: Production; // veza sa entitetom Production
}