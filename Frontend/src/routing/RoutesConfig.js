import React, { useContext } from 'react';
import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import { authRoutes, publicRoutes } from './routes';
import { AuthContext } from '../components/AuthContext';
import Navbar from '../components/Navbar';

const RoutesConfig = () => {
  const { isAuthenticated, isLoading } = useContext(AuthContext);
  const location = useLocation();  // Получаем текущий путь

  if (isLoading) {
    return <div className="flex justify-center items-center h-screen text-xl">Загрузка...</div>;
  }

  const isLoginPage = location.pathname === '/login';

  return (
    <div className="flex flex-col min-h-screen">
      {/* Показываем Navbar, если не на странице login */}
      {!isLoginPage && <Navbar />}

      <Routes>
        {isAuthenticated && authRoutes.map(({ path, Component }) => (
          <Route key={path} path={path} element={<Component />} />
        ))}

        {publicRoutes.map(({ path, Component }) => (
          <Route key={path} path={path} element={<Component />} />
        ))}

        {/* Редирект для всех неизвестных путей */}
        <Route path="*" element={<Navigate to={isAuthenticated ? "/users" : "/login"} replace />} />
      </Routes>
    </div>
  );
};

export default RoutesConfig;
