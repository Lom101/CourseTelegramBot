import React from 'react';
import './LongreadContent.css';

function LongreadContent() {
  return (
    <div className="longread-content">
      {/* Пример контента */}
      <h3>Демонстрация текста</h3>
      <p>
        Этот текст является примером того, как будет отображаться статья в лонгриде.
        Все будет красиво отформатировано и использует стильную серую тему.
      </p>
      <img src="/content/Матрица.jpg" alt="Демонстрация" />
    </div>
  );
}

export default LongreadContent;
