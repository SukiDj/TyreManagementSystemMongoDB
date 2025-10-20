import { createBrowserRouter, RouteObject } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../features/HomePage/HomePage.tsx"
import RequireQualitySupervisor from "./RequireQualitySupervisor.tsx";
import QSPage from "../features/QSPage/QSPage.tsx";
import RequireProductionOperator from "./RequireProductionOperator.tsx";
import ProductionOperatorPage from "../features/ProductionOperatorPage/ProductionOperatorPage.tsx";
import RequireBusinessUnitLeader from "./RequireBusinessUnitLeader.tsx";
import BusinessUnitLeaderPage from "../features/BusinessUnitLeaderPage/BusinessUnitLeaderPage.tsx";
import RegisterSuccess from "../features/Users/RegisterSuccess.tsx";
import ServerError from "../features/errors/ServerError";
import NotFound from "../features/errors/NotFound";

export const routes: RouteObject[] = [
    {
        path:"/",
        element: <App/>,
        children:[
            
            {
                element: <RequireQualitySupervisor />, children: [
                    {path: 'QSPage', element: <QSPage /> }
                ]
            },
            {
                element: <RequireProductionOperator />, children: [
                    {path: 'ProductionOperatorPage', element:<ProductionOperatorPage/>}
                ]
            },
            {
                element: <RequireBusinessUnitLeader />, children: [
                    {path: 'BusinessUnitLeaderPage', element:<BusinessUnitLeaderPage/>}
                ]
            },

            {path:'', element:<HomePage />},
            {path:'/RegisterSuccess', element:<RegisterSuccess />},
            { path: '/server-error', element: <ServerError /> },
            { path: '/not-found', element: <NotFound /> }
        ]
    }
    
]

export const router = createBrowserRouter(routes);  