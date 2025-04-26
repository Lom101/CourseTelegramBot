import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext';
import RoutesConfig from './routing/RoutesConfig';
import { Toaster } from 'react-hot-toast';

function App() {
  return (
    <Router>
      <Toaster position="top-right" />
      <RoutesConfig />
    </Router>
  );
}

export default App;
