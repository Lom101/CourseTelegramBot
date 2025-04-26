import React, { createContext, useState, useEffect, useCallback } from 'react';
import axios from 'axios';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [token, setToken] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      setToken(storedToken);
      axios.get(`${process.env.REACT_APP_API_URL}/api/Auth/check`, {
        headers: { Authorization: `Bearer ${storedToken}` },
      })
      .then(() => setIsAuthenticated(true))
      .catch(() => logout())
      .finally(() => setIsLoading(false));
    } else {
      setIsLoading(false);
    }
  }, []);

  const login = (userToken) => {
    setIsAuthenticated(true);
    setToken(userToken);
    localStorage.setItem('token', userToken);
  };

  const logout = () => {
    setIsAuthenticated(false);
    setToken(null);
    localStorage.removeItem('token');
  };

  const checkAuth = useCallback(async (existingToken) => {
    try {
      const tokenToCheck = existingToken || token;
      const response = await axios.get(`${process.env.REACT_APP_API_URL}/api/Auth/check`, {
        headers: { Authorization: `Bearer ${tokenToCheck}` },
      });
      if (response.status === 200) {
        setIsAuthenticated(true);
        return true;
      }
    } catch (error) {
      logout();
    }
    return false;
  }, [token]);

  return (
    <AuthContext.Provider value={{ isAuthenticated, token, login, logout, checkAuth, isLoading }}>
      {children}
    </AuthContext.Provider>
  );
};
