import { makeAutoObservable, runInAction } from "mobx";
import { RegisterUserFormValues, User, UserFormValues } from "../models/User";
import { store } from "./store";
import { router } from "../router/Routes";
import agent from "../../api/agent";


export default class userStore{
    user: User | null = null;
    refreshTokenTimeout?: number;
    registerNow : boolean = false;
    userRegistry = new Map<string, User>();

    constructor(){
        makeAutoObservable(this);
    }

    setUserForRegistry = (user: User) => {
        this.userRegistry.set(user.userName, user);
    }

    getAllOperators = async () => {
        try {
            const users = await agent.Account.getOperators();
            runInAction(() => {
                this.userRegistry.clear();
                users.forEach(user => this.setUserForRegistry(user));
            });
        } catch (error) {
            console.log(error);
        }
    }

    get OperatorsOptions() {
        return Array.from(this.userRegistry.values()).map((user) => ({
          key: user.id,
          text: user.userName,
          value: user.id
        }));
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

    getUser = async () =>{
        try{
            const user = await agent.Account.current();
            store.commonStore.setToken(user.token);//uvek kad dobijemo usera sa servera setujemo token i startujemo timer
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
            runInAction(()=> this.user = user);
        } catch(error){
            console.log(error);
        }
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
    setUser = (user: User) => {
        this.user = user;
        window.localStorage.setItem('jwt', user.token);   
        store.commonStore.token = user.token;             
    };
      refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try {
            const user = await agent.Account.refreshToken(); // POST /Account/refreshToken (Authorized)
            runInAction(() => (this.user = user));
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);
        } catch (err) {
        console.log(err);
        }
    };

    private startRefreshTokenTimer(user: User){
        const jwToken = JSON.parse(atob(user.token.split('.')[1]));//atob vadi info iz tokena
        const expires = new Date(jwToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (30 * 1000);//tajmaut od 30 sekunde
        this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);//kad istekne token pozivam ovu metodu iznad da nam server da novi token
        console.log({refreshTimeout: this.refreshTokenTimeout});
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout);
    }
}
