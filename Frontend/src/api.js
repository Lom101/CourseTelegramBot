import axios from 'axios';
import { AuthContext } from './components/AuthContext';
import { useContext } from 'react';

const useApi = () => {
  const { token } = useContext(AuthContext);

  const api = axios.create({
    baseURL: process.env.REACT_APP_API_URL,
  });

  // Добавляем токен в каждый запрос
  api.interceptors.request.use((config) => {
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  }, (error) => {
    return Promise.reject(error);
  });

  return api;
};

export default useApi;