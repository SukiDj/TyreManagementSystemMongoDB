import { makeAutoObservable, runInAction } from 'mobx';
import agent from '../../api/agent'; 
import { ActionLog } from '../models/ActionLog';  // Uvoz novog modela

export default class LogStore {
    logs: ActionLog[] = []; // Lista logova sada koristi tip ActionLog
    loadingLogs = false;

    constructor() {
        makeAutoObservable(this);
    }

    loadLogs = async (): Promise<ActionLog[]> => {
        this.loadingLogs = true;
        try {
            const logs = await agent.Logs.list(); 
            runInAction(() => {
                this.logs = logs; // Postavljanje stanja sa nizom ActionLog objekata
                this.loadingLogs = false;
            });
            return logs;
        } catch (error) {
            console.error("Failed to load logs", error);
            runInAction(() => {
                this.loadingLogs = false;
            });
            return [];
        }
    }
}
