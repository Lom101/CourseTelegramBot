import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../components/AuthContext';
import axios from 'axios';

const LoginPage = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { login: doLogin } = useContext(AuthContext);

  console.log('API URL:', process.env.REACT_APP_API_URL);

  const handleLogin = async (e) => {
    e.preventDefault();
  
    try {
      const response = await axios.post(
        `${process.env.REACT_APP_API_URL}/api/Auth/admin-login`,
        {
          email: login,
          password,
        }
      );

      if (response.data.token) {
        doLogin(response.data.token);
        navigate('/users');
      } else {
        alert('Неверный логин или пароль');
      }
    } catch (error) {
      if (error.response) {
        console.error('Ответ от сервера:', error.response.data);
        alert(`Ошибка: ${error.response.status} - ${JSON.stringify(error.response.data)}`);
      } else if (error.request) {
        console.error('Нет ответа от сервера. Запрос был отправлен:', error.request);
        alert('Сервер не ответил. Проверь соединение.');
      } else {
        console.error('Ошибка при настройке запроса:', error.message);
        alert(`Ошибка: ${error.message}`);
      }
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100 px-4">
      <div className="bg-white p-8 rounded-2xl shadow-md w-full max-w-md">
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-700">Вход в админ-панель</h2>
        <form onSubmit={handleLogin} className="space-y-4">
          <div>
            <label htmlFor="login" className="block text-gray-600 mb-1 font-semibold">
              Логин:
            </label>
            <input
              type="text"
              id="login"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
              required
              className="w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 focus:ring-gray-300"
            />
          </div>
          <div>
            <label htmlFor="password" className="block text-gray-600 mb-1 font-semibold">
              Пароль:
            </label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="w-full px-4 py-2 border rounded-xl focus:outline-none focus:ring-2 focus:ring-gray-300"
            />
          </div>
          <button
            type="submit"
            className="w-full bg-gray-700 hover:bg-gray-800 text-white font-semibold py-2 rounded-xl transition"
          >
            Войти
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;