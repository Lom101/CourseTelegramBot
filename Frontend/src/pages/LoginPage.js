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
    <div className="min-h-screen flex items-center justify-center bg-[#b9bedf]">
      <div className="login-card">
        <h2 className="title">Вход в админ-панель</h2>
        <form onSubmit={handleLogin} className="form">
          <div className="form-group">
            <label htmlFor="login" className="label">Логин:</label>
            <input
              type="text"
              id="login"
              value={login}
              onChange={(e) => setLogin(e.target.value)}
              required
              className="input"
            />
          </div>
          <div className="form-group">
            <label htmlFor="password" className="label">Пароль:</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              className="input"
            />
          </div>
          <button type="submit" className="button">
            Войти
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;