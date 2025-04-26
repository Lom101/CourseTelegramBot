import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext';
import RoutesConfig from './routing/RoutesConfig';

function App() {
  return (
    <AuthProvider>
      <Router>
        <RoutesConfig />
      </Router>
    </AuthProvider>
  );
}

export default App;
