import React, { useContext } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MaterialsPage from './pages/MaterialsPage';
import UsersPage from './pages/UsersPage';
import BlockPage from './pages/BlockPage';
import Navbar from './components/Navbar';
import LoginPage from './pages/LoginPage';
import { AuthContext } from './components/AuthContext';

// Компонент для защищённых маршрутов
const PrivateRoute = ({ element }) => {
  const { isAuthenticated } = useContext(AuthContext);
  return isAuthenticated ? element : <Navigate to="/login" replace />;
};

// Компонент для публичных маршрутов (например, логин)
const PublicRoute = ({ element }) => {
  const { isAuthenticated } = useContext(AuthContext);
  return !isAuthenticated ? element : <Navigate to="/users" replace />;
};

function App() {
  const { isAuthenticated } = useContext(AuthContext);

  return (
    <Router>
      <div className="min-h-screen bg-page-gray">
        {isAuthenticated && <Navbar />}
        <main className="p-6">
          <Routes>
            <Route path="/login" element={<PublicRoute element={<LoginPage />} />} />
            <Route path="/users" element={<PrivateRoute element={<UsersPage />} />} />
            <Route path="/materials" element={<PrivateRoute element={<MaterialsPage />} />} />
            <Route path="/materials/block/:id" element={<PrivateRoute element={<BlockPage />} />} />
            <Route
              path="*"
              element={<Navigate to={isAuthenticated ? "/users" : "/login"} replace />}
            />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;