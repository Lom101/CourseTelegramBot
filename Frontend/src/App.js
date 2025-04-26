import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/AuthContext';
import RoutesConfig from './routing/RoutesConfig';

function App() {
  return (
    <Router>
      <RoutesConfig />
    </Router>
  );
}

export default App;
