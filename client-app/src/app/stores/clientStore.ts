import { makeAutoObservable } from 'mobx';
import { Client } from '../models/Client';
import agent from '../../api/agent';

export default class clientStore {
  clientRegistry = new Map<string, Client>();
  loadingInitial = false;

  constructor() {
    makeAutoObservable(this);
  }

  setLoadingInitial = (state: boolean) => {
    this.loadingInitial = state;
  };

  setClient = (client: Client) => {
    this.clientRegistry.set(client.id, client);  // Koristimo client.id kao ključ
  };

  loadClients = async () => {
    this.setLoadingInitial(true);
    try {
      const clients: Client[] = await agent.Clients.getClients();  // Pretpostavljam da imaš agenta za klijente
      
      clients.forEach(client => {
        this.setClient(client);
      });
      
      this.setLoadingInitial(false);
    } catch (error) {
      console.log(error);
      this.setLoadingInitial(false);
    }
  };

  get clientOptions() {
    return Array.from(this.clientRegistry.values()).map((client) => ({
      key: client.id,       // Unique key based on client ID
      text: client.name,    // Displayed in the dropdown (možda ime klijenta)
      value: client.id,     // Value to be selected (ID klijenta)
    }));
  }
}
