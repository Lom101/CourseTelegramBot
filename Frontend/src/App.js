import React, { useContext } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MaterialsPage from './pages/MaterialsPage';
import UsersPage from './pages/UsersPage';
import BlockPage from './pages/BlockPage';
import Navbar from './components/Navbar';
import LoginPage from './pages/LoginPage';
import { AuthContext } from './components/AuthContext';

function App() {
  const { isAuthenticated } = useContext(AuthContext);

  return (
    <Router>
      <div className="min-h-screen bg-page-gray">
        {isAuthenticated && <Navbar />}
        <main className="p-6">
          <Routes>
            <Route path="/login" element={<LoginPage />} />
            {isAuthenticated ? (
              <>
                <Route path="/users" element={<UsersPage />} />
                <Route path="/materials" element={<MaterialsPage />} />
                <Route path="/materials/block/:id" element={<BlockPage />} />
                <Route path="*" element={<Navigate to="/users" />} />
              </>
            ) : (
              <Route path="*" element={<Navigate to="/login" />} />
            )}
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;