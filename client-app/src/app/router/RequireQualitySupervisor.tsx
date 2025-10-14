import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";

export default function RequireAdmin (){
    const {userStore : {isLoggedIn, isQualitySupervisor}} = useStore();
    const location = useLocation();

    if(!isLoggedIn) {
        return <Navigate to='' state={{from: location}} />
    }
    if(!isQualitySupervisor){
        return <Navigate to='/not-found' state={{from: location}} />
    }

    return <Outlet />
}