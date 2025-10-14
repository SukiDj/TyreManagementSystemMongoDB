import { createContext, useContext } from "react";
import userStore from "./userStore";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import BusinessUnitStore from "./businessUnitStore";
import RecordStore from "./productionRecordStore";
import tyreStore from "./tyreStore";
import machineStore from "./machineStore";
import productionRecordStore from "./productionRecordStore";
import saleRecordStore from "./saleRecordStore";
import clientStore from "./clientStore";
import stockRecordStore from "./stockRecordStore";
import logStore from "./logStore";

interface Store{
    commonStore : CommonStore;
    modalStore: ModalStore;
    userStore: userStore;
    recordStore: RecordStore
    businessUnitStore: BusinessUnitStore;
    tyreStore: tyreStore;
    machineStore: machineStore;
    clientStore: clientStore;
    productionRecordStore: productionRecordStore;
    saleRecordStore: saleRecordStore;
    stockRecordStore: stockRecordStore;
    logStore: logStore;
}

export const store: Store = {
    commonStore : new CommonStore(),
    modalStore: new ModalStore(),
    userStore: new userStore(),
    businessUnitStore: new BusinessUnitStore(),
    recordStore: new RecordStore(),
    tyreStore: new tyreStore(),
    machineStore: new machineStore(),
    clientStore: new clientStore(),
    productionRecordStore: new productionRecordStore(),
    saleRecordStore: new saleRecordStore(),
    stockRecordStore: new stockRecordStore(),
    logStore: new logStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}