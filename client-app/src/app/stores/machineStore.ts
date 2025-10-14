import { makeAutoObservable } from 'mobx';
import agent from '../../api/agent';
import { Machine } from '../models/Machine';

export default class tyreStore {
  machineRegistry = new Map<string, Machine>();
  loadingInitial = false;


  constructor(){
    makeAutoObservable(this);
}
setLoadingInitial = (state: boolean) =>{
    this.loadingInitial = state;
}

setMachine = (machine: Machine) => {
    this.machineRegistry.set(machine.id, machine);
}
loadMachines = async () => {
    this.setLoadingInitial(true);
    try {
        const machines :Machine[] = await agent.Machines.getMachines();
        
        machines.forEach(machine => {
                this.setMachine(machine);
            });
            this.setLoadingInitial(false);
            
    } catch (error) {
        console.log(error);
       
            this.setLoadingInitial(false);
        
    }
}
get machineOptions() {
    return Array.from(this.machineRegistry.values()).map((machine) => ({
      key: machine.id,      
      text: machine.name,     
      value: machine.id,    
    }));
  }
}
