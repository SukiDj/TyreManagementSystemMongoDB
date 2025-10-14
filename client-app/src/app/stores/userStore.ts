import { makeAutoObservable, runInAction } from "mobx";
import { RegisterUserFormValues, User, UserFormValues } from "../models/User";
import { store } from "./store";
import { router } from "../router/Routes";
import agent from "../../api/agent";


export default class userStore{
    user: User | null = null;
    refreshTokenTimeout?: number;
    registerNow : boolean = false;


    constructor(){
        makeAutoObservable(this);
    }

    RegisterNow = () => this.registerNow = true;

    get isLoggedIn(){
        return !!this.user;
    }

    getDecodedToken(token : string){
        return JSON.parse(atob(token.split('.')[1]))// delimo token po . jer je prvi deo algoritam, pa drugi deo iza tacke nama bitne informacije, pa treci deo iza tacke potpis sto nas ne zanima, pa sa [1] uzimamo samo informacije koje nas zanimaju
    }

    get isProductionOperator() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("ProductionOperator");
    }

    get isBusinessUnitLeader() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("BusinessUnitLeader");
    }

    get isQualitySupervisor() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("QualitySupervisor");
    }

    login = async (creds: UserFormValues) => {
        try{
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
            runInAction(()=> this.user = user);
            if(this.isQualitySupervisor)
                router.navigate('/QSPage');
            if(this.isProductionOperator)
                router.navigate('/ProductionOperatorPage');
            if(this.isBusinessUnitLeader)
                router.navigate('/BusinessUnitLeaderPage');
            
            store.modalStore.closeModal();
        } catch(error){
            throw error;
        }
        
    }

    loguot = () =>{
        store.commonStore.setToken(null);
        this.user = null;
        router.navigate('/');
    }

    register = async (podaci: RegisterUserFormValues) => {
        try{
            await agent.Account.register(podaci);
            console.log(podaci)
            router.navigate(`/RegisterSuccess`);
            store.modalStore.closeModal();
        } catch(error){
            throw error;
        }
        
    }


    private startRefreshTokenTimer(user: User){
        const jwToken = JSON.parse(atob(user.token.split('.')[1]));//atob vadi info iz tokena
        const expires = new Date(jwToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (30 * 1000);//tajmaut od 30 sekunde
        this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);//kad istekne token pozivam ovu metodu iznad da nam server da novi token
        console.log({refreshTimeout: this.refreshTokenTimeout});
    }

    refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try
        {
            const user = await agent.Account.refreshToken();
            runInAction(() => this.user = user);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
        }
        catch(error)
        {
            console.log(error);
        }
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout)
    }

}
