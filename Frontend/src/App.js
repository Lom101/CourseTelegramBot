// import React from 'react';
// import AdminDashboard from './pages/AdminDashboard';

// function App() {
//   return (
//     <div>
//       <AdminDashboard />
//     </div>
//   );
// }

// export default App;

import React, {useEffect, useState} from 'react';
import './styles/App.css';
import './styles/index.css';
import {BrowserRouter} from "react-router-dom";
import Navbar from "./components/UI/Navbar/Navbar";
import AdminDashboard from "./pages/AdminDashboard";
import {AuthContext} from "./context";

function App() {
    const [isAuth, setIsAuth] = useState(false);
    const [isLoading, setLoading] = useState(true);

    useEffect(() => {
        if (localStorage.getItem('auth')) {
            setIsAuth(true)
        }
        setLoading(false);
    }, [])

    return (
        <AuthContext.Provider value={{
            isAuth,
            setIsAuth,
            isLoading
        }}>
            <BrowserRouter>
                <Navbar/>
                <AdminDashboard/>
            </BrowserRouter>
        </AuthContext.Provider>
    )
}

export default App;