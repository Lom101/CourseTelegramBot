import React from 'react';
import LongreadPage from './components/LongreadPage/LongreadPage';  // Импортируем компонент LongreadPage

function App() {
  return (
    <div className="App">
      <h1>Контент</h1>
      
      <h2>Получить контент по ID</h2>
      <LongreadPage id={1} /> {/* Передаем id прямо в LongreadPage */}
    </div>
  );
}

export default App;