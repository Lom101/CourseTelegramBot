import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MaterialsPage from './pages/MaterialsPage';
import UsersPage from './pages/UsersPage';
import BlockPage from './pages/BlockPage';
import Navbar from './components/Navbar';

function App() {
  return (
    <Router>
      <div className="min-h-screen bg-page-gray">
      <Navbar />

        <main className="p-6">
          <Routes>
            <Route path="/" element={<Navigate to="/users" />} />
            <Route path="/users" element={<UsersPage />} />
            <Route path="/materials" element={<MaterialsPage />} />
            <Route path="/materials/block/:id" element={<BlockPage />} />
            <Route path="*" element={<Navigate to="/users" />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;