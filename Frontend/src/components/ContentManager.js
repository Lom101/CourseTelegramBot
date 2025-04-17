import React from 'react';
import ContentItem from './ContentItem';

const formatFileName = (filename) => {
  const nameWithoutExt = filename.replace(/\.[^/.]+$/, '');
  const withSpaces = nameWithoutExt.replace(/_/g, ' ');
  const withColons = withSpaces.replace(/(Лонгрид\s*\d+)\./i, '$1:');
  return withColons.replace(/\s{2,}/g, ' ').trim();
};

const FILES = {
  block1: {
    audios: ['Аудио.m4a'],
    books: ['Сверх продуктивность, Михаил Алистер.pdf', 'Тайм-менеджмент, Брайан Трейси.pdf'],
    pictures: ['метод АБВГД.jpg', 'метод Матрица Эйзенхауэра.jpg'],
    texts: ['1 Лонгрид. тайм-менеджмент тим лида.docx', '2 Лонгрид. Матрица Эйзенхауэра.docx', '3 Лонгрид. ABCDE (АБВГД).docx']
  },
  block2: {
    books: ['Пять_пороков_команды_Патрик Ленсиони.pdf', 'Теория U_Отто Шармер.pdf'],
    pictures: ['картинка к лонгриду 2 (1).png', 'картинка к лонгриду 2.png'],
    texts: ['1 Лонгрид. Кто такой лидер.docx', '2 Лонгрид. Стили лидерства.docx']
  },
  block3: {
    audios: ['Делегирование.m4a'],
    books: ['Делегирование и управление_Трейси Б..pdf', 'Делегирование. Фридман.pdf'],
    pictures: ['Модель HD-RW-RM.jpg', 'Модель SMART.jpg', 'Модель TOTE.jpg'],
    texts: ['Лонгрид 1. Постановка задач (модели).docx', 'Лонгрид 2. Делегирование.docx']
  },
  block4: {
    audios: ['Культура совместной работы. правила тимлида.m4a'],
    books: ['Пять_пороков_команды_Притчи_о_лидерстве.pdf'],
    texts: ['Лонгрид 1. Культура совместной работы.docx', 'Лонгрид 2. Роли в команде.docx']
  }
};

const ContentManager = ({ blockId }) => {
  const blockData = FILES[blockId];
  if (!blockData) return null;

  const renderItems = (type, folderName) => {
    if (!blockData[folderName]) return null;

    return blockData[folderName].map((filename, index) => (
      <ContentItem
        key={`${blockId}-${type}-${index}`}
        type={type}
        title={formatFileName(filename)}
        path={`/blocks/${blockId}/${folderName}/${filename}`}
      />
    ));
  };

  return (
    <div style={{ border: '1px solid #ccc', padding: '1rem', marginBottom: '2rem', borderRadius: '10px' }}>
      <h2>Материалы {blockId}</h2>
      {renderItems('audio', 'audios')}
      {renderItems('book', 'books')}
      {renderItems('picture', 'pictures')}
      {renderItems('text', 'texts')}
    </div>
  );
};

export default ContentManager;