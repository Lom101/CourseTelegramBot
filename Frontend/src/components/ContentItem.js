import React, { useState } from 'react';

const icons = {
  text: "📄",
  picture: "🖼️",
  audio: "🎵",
  book: "📘"
};

const ContentItem = ({ type, title, path, showAddButton }) => {
  const [isEditing, setIsEditing] = useState(false);
  void(isEditing);
  const [isDeleted, setIsDeleted] = useState(false);

  if (isDeleted) return null;

  const handleAdd = () => {
    setIsEditing(true);
    window.confirm(`Добавить?`);
    setIsEditing(false);
  };

  const handleEdit = () => {
    setIsEditing(true);
    setTimeout(() => {
      alert(`Редактирование: ${title}`);
      setIsEditing(false);
    }, 500);
  };

  const handleDelete = () => {
    if (window.confirm(`Удалить: ${title}?`)) {
      setIsDeleted(true);
    }
  };

  const handleDownload = () => {
    const link = document.createElement('a');
    link.href = path;
    link.setAttribute('download', title);
    document.body.appendChild(link);
    link.click();
    link.remove();
  };

  const renderAddButton = () => (
    showAddButton && (
      <div className="mt-4 border-t pt-4">
        <button
          onClick={handleAdd}
          className="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600"
        >
          Добавить
        </button>
      </div>
    )
  );

  if (type === 'picture') {
    return (
      <div className="border rounded-lg p-3 shadow bg-white mb-4">
        <p className="font-semibold mb-2">{icons.picture} {title}</p>
        <a href={path} target="_blank" rel="noopener noreferrer">
          <img
            src={path}
            alt={title}
            className="w-full h-auto max-h-60 object-contain rounded"
            onError={(e) => (e.target.style.display = 'none')}
          />
        </a>
        <div className="mt-2 flex gap-3">
          <button onClick={handleEdit} className="text-blue-600 hover:underline">Изменить</button>
          <button onClick={handleDelete} className="text-red-600 hover:underline">Удалить</button>
        </div>
        {renderAddButton()}
      </div>
    );
  }

  if (type === 'audio') {
    return (
      <div className="border rounded-lg p-3 shadow bg-white mb-4">
        <p className="font-semibold mb-2">{icons.audio} {title}</p>
        <audio controls className="w-full">
          <source src={path} type="audio/mp4" />
          Ваш браузер не поддерживает аудио.
        </audio>
        <div className="mt-2 flex gap-3">
          <button onClick={handleEdit} className="text-blue-600 hover:underline">Изменить</button>
          <button onClick={handleDelete} className="text-red-600 hover:underline">Удалить</button>
        </div>
        {renderAddButton()}
      </div>
    );
  }

  // Для text и book (скачиваемые материалы)
  return (
    <div className="border rounded-lg p-3 shadow bg-white mb-4">
      <p className="font-semibold mb-1">{icons[type]} {title}</p>
      <button
        onClick={handleDownload}
        className="text-blue-600 hover:underline"
      >
        ⬇️ Скачать
      </button>
      <div className="mt-2 flex gap-3">
        <button onClick={handleEdit} className="text-blue-600 hover:underline">Изменить</button>
        <button onClick={handleDelete} className="text-red-600 hover:underline">Удалить</button>
      </div>
      {renderAddButton()}
    </div>
  );
};

export default ContentItem;