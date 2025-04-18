import React from 'react';
import ContentItem from './ContentItem';

const FILES = {
  block1: {
    audios: ['Аудио.m4a'],
    books: [
      'Сверх продуктивность, Михаил Алистер.pdf',
      'Тайм-менеджмент, Брайан Трейси.pdf'
    ],
    pictures: ['метод АБВГД.jpg', 'метод Матрица Эйзенхауэра.jpg'],
    texts: [
      '1 Лонгрид. тайм-менеджмент тим лида.docx',
      '2 Лонгрид. Матрица Эйзенхауэра.docx',
      '3 Лонгрид. ABCDE (АБВГД).docx'
    ]
  },
  block2: {
    books: [
      'Пять_пороков_команды_Патрик Ленсиони.pdf',
      'Теория U_Отто Шармер.pdf'
    ],
    pictures: ['картинка к лонгриду 2 (1).png', 'картинка к лонгриду 2.png'],
    texts: [
      '1 Лонгрид. Кто такой лидер.docx',
      '2 Лонгрид. Стили лидерства.docx'
    ]
  },
  block3: {
    audios: ['Делегирование.m4a'],
    books: [
      'Делегирование и управление_Трейси Б..pdf',
      'Делегирование. Фридман.pdf'
    ],
    pictures: ['Модель HD-RW-RM.jpg', 'Модель SMART.jpg', 'Модель TOTE.jpg'],
    texts: [
      'Лонгрид 1. Постановка задач (модели).docx',
      'Лонгрид 2. Делегирование.docx'
    ]
  },
  block4: {
    audios: ['Культура совместной работы. правила тимлида.m4a'],
    books: ['Пять_пороков_команды_Притчи_о_лидерстве.pdf'],
    texts: [
      'Лонгрид 1. Культура совместной работы.docx',
      'Лонгрид 2. Роли в команде.docx'
    ]
  }
};

const formatFileName = (filename) => {
  const withoutExt = filename.replace(/\.[^/.]+$/, '');
  const withSpaces = withoutExt.replace(/[_-]+/g, ' ');
  return withSpaces.charAt(0).toUpperCase() + withSpaces.slice(1);
};

const ContentManager = ({ blockId }) => {
  const blockData = FILES[`block${blockId}`];
  if (!blockData) return null;

  const renderItems = (type, folderName) => {
    const items = blockData[folderName];
    if (!items) return null;

    return items.map((filename, index) => {
      const encodedPath = encodeURIComponent(filename);
      return (
        <ContentItem
          key={`${blockId}-${type}-${index}`}
          type={type}
          title={formatFileName(filename)}
          path={`/blocks/${blockId}/${folderName}/${encodedPath}`}
        />
      );
    });
  };

  return (
    <div className="space-y-4 border p-4 rounded bg-gray-50 shadow">
      {renderItems('audio', 'audios')}
      {renderItems('book', 'books')}
      {renderItems('picture', 'pictures')}
      {renderItems('text', 'texts')}

      {/* ОДНА кнопка "Добавить" внизу блока */}
      <div className="pt-4 border-t mt-4 text-center">
        <button
          onClick={() => window.confirm(`Добавить материал в блок ${blockId}?`)}
          className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600"
        >
          Добавить
        </button>
      </div>
    </div>
  );
};

export default ContentManager;