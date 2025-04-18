import AdminDashboard from "../pages/AdminDashboard";
import Login from "../pages/Login";


export const privateRoutes = [
    {path: '/admin', component: AdminDashboard, exact: true},
]

export const publicRoutes = [
    {path: '/login', component: Login, exact: true},
]