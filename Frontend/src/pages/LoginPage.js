import React, { useState } from 'react';
import '../index.css'; // Импортируем стили

const LoginPage = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = (e) => {
    e.preventDefault();
    // Здесь можно добавить логику для отправки данных на сервер
    console.log('Логин:', login);
    console.log('Пароль:', password);
    alert('Форма отправлена!');
  };

  return (
    <div className="container">
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