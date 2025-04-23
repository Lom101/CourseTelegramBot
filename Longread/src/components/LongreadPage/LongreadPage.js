import React, { useState, useEffect } from 'react';
import Word from '../Word';
import './LongreadPage.css';

function LongreadPage() {
  const [docContent, setDocContent] = useState('');

  useEffect(() => {
    const fetchWordFile = async () => {
      const response = await fetch('/content/report3.docx');
      const arrayBuffer = await response.arrayBuffer();

      const text = await Word.convertToHTML(arrayBuffer);
      setDocContent(text);
    };

    fetchWordFile();
  }, []);

  return (
    <div className="longread-page">
      <div className="header">
        <img src="/content/logo192.jpg" alt="Логотип 1" className="logo left-logo" />
        <h1>Блок 1. Навык Тайм-менеджмент</h1>
        <h2>Матрица Эйзенхауэра</h2>
      </div>
      <div className="content">
        <div className="article">
          <div dangerouslySetInnerHTML={{ __html: docContent }} />
        </div>
        <div className="image-container">
          <img src="/content/Матрица.jpg" alt="Тема тайм-менеджмента" />
        </div>
      </div>
    </div>
  );
  
}

export default LongreadPage;
