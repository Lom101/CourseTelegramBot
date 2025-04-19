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

    return (
      <div className="mb-8">
        <h2 className="text-lg font-semibold text-[#2E2E2E] mb-3 capitalize">
          {folderName}
        </h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
          {items.map((filename, index) => {
            const encodedPath = encodeURIComponent(filename);
            return (
              <div
                key={`${blockId}-${type}-${index}`}
                className="bg-white rounded-2xl shadow p-4 flex flex-col justify-between border border-[#E7E5E4] hover:shadow-md transition"
              >
                <ContentItem
                  type={type}
                  title={formatFileName(filename)}
                  path={`/blocks/block${blockId}/${folderName}/${encodedPath}`}
                />
              </div>
            );
          })}
        </div>
      </div>
    );
  };

  return (
    <div className="bg-[#F9F7F4] rounded-3xl shadow-lg p-6 space-y-6 border border-[#EDEDED]">
      {renderItems('audio', 'audios')}
      {renderItems('book', 'books')}
      {renderItems('picture', 'pictures')}
      {renderItems('text', 'texts')}

      <div className="pt-4 border-t border-[#E7E5E4] text-center">
        <button
          onClick={() => window.confirm(`Добавить материал в блок ${blockId}?`)}
          className="mt-4 px-5 py-2 rounded-xl bg-[#BFD7ED] text-[#2E2E2E] hover:bg-[#A6C5E2] transition text-sm font-medium inline-flex items-center justify-center gap-2"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          >
            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
            <path d="M9 5h-2a2 2 0 0 0 -2 2v12a2 2 0 0 0 2 2h10a2 2 0 0 0 2 -2v-12a2 2 0 0 0 -2 -2h-2" />
            <path d="M9 3m0 2a2 2 0 0 1 2 -2h2a2 2 0 0 1 2 2v0a2 2 0 0 1 -2 2h-2a2 2 0 0 1 -2 -2z" />
            <path d="M10 14h4" />
            <path d="M12 12v4" />
          </svg>
          <span>Добавить материал</span>
        </button>
      </div>
    </div>
  );
};

export default ContentManager;