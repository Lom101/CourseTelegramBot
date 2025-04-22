import React, { useState, useEffect } from 'react';

const API_URL = process.env.API_URL;

const icons = {
  text: <svg className="w-5 h-5 inline mr-2" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12h6M9 16h6M9 5h-2a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2m-6 0a2 2 0 114 0 2 2 0 01-4 0z" /></svg>,
  picture: <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="icon icon-tabler icons-tabler-outline icon-tabler-library-photo"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M7 3m0 2.667a2.667 2.667 0 0 1 2.667 -2.667h8.666a2.667 2.667 0 0 1 2.667 2.667v8.666a2.667 2.667 0 0 1 -2.667 2.667h-8.666a2.667 2.667 0 0 1 -2.667 -2.667z" /><path d="M4.012 7.26a2.005 2.005 0 0 0 -1.012 1.737v10c0 1.1 .9 2 2 2h10c.75 0 1.158 -.385 1.5 -1" /><path d="M17 7h.01" /><path d="M7 13l3.644 -3.644a1.21 1.21 0 0 1 1.712 0l3.644 3.644" /><path d="M15 12l1.644 -1.644a1.21 1.21 0 0 1 1.712 0l2.644 2.644" /></svg>,
  audio: <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="icon icon-tabler icons-tabler-outline icon-tabler-device-audio-tape"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M3 5m0 2a2 2 0 0 1 2 -2h14a2 2 0 0 1 2 2v10a2 2 0 0 1 -2 2h-14a2 2 0 0 1 -2 -2z" /><path d="M3 17l4 -3h10l4 3" /><circle cx="7.5" cy="9.5" r=".5" fill="currentColor" /><circle cx="16.5" cy="9.5" r=".5" fill="currentColor" /></svg>,
  book: <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" className="icon icon-tabler icons-tabler-outline icon-tabler-books"><path stroke="none" d="M0 0h24v24H0z" fill="none"/><path d="M5 4m0 1a1 1 0 0 1 1 -1h2a1 1 0 0 1 1 1v14a1 1 0 0 1 -1 1h-2a1 1 0 0 1 -1 -1z" /><path d="M9 4m0 1a1 1 0 0 1 1 -1h2a1 1 0 0 1 1 1v14a1 1 0 0 1 -1 1h-2a1 1 0 0 1 -1 -1z" /><path d="M5 8h4" /><path d="M9 16h4" /><path d="M13.803 4.56l2.184 -.53c.562 -.135 1.133 .19 1.282 .732l3.695 13.418a1.02 1.02 0 0 1 -.634 1.219l-.133 .041l-2.184 .53c-.562 .135 -1.133 -.19 -1.282 -.732l-3.695 -13.418a1.02 1.02 0 0 1 .634 -1.219l.133 -.041z" /><path d="M14 9l4 -1" /><path d="M16 16l3.923 -.98" /></svg>
};

const ContentItem = ({ id, type, title, path }) => {
  const [isDeleted, setIsDeleted] = useState(false);
  const [textContent, setTextContent] = useState('');
  const [isEditing, setIsEditing] = useState(false);
  const [editedText, setEditedText] = useState('');

  useEffect(() => {
    if (type === 'text') {
      fetch(`${API_URL}/${path}`)
        .then((res) => res.text())
        .then((text) => {
          setTextContent(text);
          setEditedText(text);
        })
        .catch((err) => setTextContent('Ошибка загрузки текста.'));
    }
  }, [type, path]);

  if (isDeleted) return null;

  const handleEdit = () => {
    setIsEditing(true);
  };

  const handleSave = () => {
    fetch(`${API_URL}/texts/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ content: editedText }),
    })
      .then((res) => {
        if (!res.ok) throw new Error('Ошибка при сохранении');
        return res.text();
      })
      .then(() => {
        setTextContent(editedText);
        setIsEditing(false);
      })
      .catch((err) => alert('Ошибка при сохранении текста'));
  };

  const handleDelete = async () => {
    if (window.confirm(`Удалить: ${title}?`)) {
      try {
        const response = await fetch('/api/delete-file', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ path }),
        });
        if (response.ok) {
          setIsDeleted(true);
        } else {
          alert('Ошибка при удалении файла.');
        }
      } catch (error) {
        console.error('Ошибка при удалении:', error);
      }
    }
  };

  return (
    <div className="bg-white shadow rounded-xl p-4 border hover:shadow-md transition-all whitespace-pre-wrap">
      <div className="flex items-center justify-between mb-3">
        <h4 className="text-lg font-semibold text-gray-800 flex items-center">
          {icons[type]} {title}
        </h4>
        <div className="flex gap-2">
          {!isEditing && type === 'text' && (
            <button onClick={handleEdit} className="text-blue-600 hover:text-blue-800" title="Изменить">
              ✎
            </button>
          )}
          <button onClick={handleDelete} className="text-red-600 hover:text-red-800" title="Удалить">
            ✖
          </button>
        </div>
      </div>

      {type === 'picture' ? (
        <img src={`${API_URL}/${path}`} alt={title} className="rounded-md w-full max-h-60 object-contain" />
      ) : type === 'audio' ? (
        <audio controls className="w-full">
          <source src={`${API_URL}/${path}`} type="audio/mpeg" />
          <source src={`${API_URL}/${path}`} type="audio/x-m4a" />
          Ваш браузер не поддерживает аудио.
        </audio>
      ) : type === 'book' ? (
        <a href={`${API_URL}/${path}`} download className="text-sm mt-1 px-3 py-1 inline-block rounded bg-blue-50 text-blue-600 hover:bg-blue-100">
          Скачать
        </a>
      ) : type === 'text' ? (
        isEditing ? (
          <div className="mt-2 text-sm">
            <textarea className="w-full border rounded p-2" rows={10} value={editedText} onChange={(e) => setEditedText(e.target.value)} />
            <button onClick={handleSave} className="mt-2 bg-blue-600 text-white px-4 py-1 rounded hover:bg-blue-700">
              Сохранить
            </button>
          </div>
        ) : (
          <div className="mt-2 text-gray-700 text-sm" dangerouslySetInnerHTML={{ __html: textContent }} />
        )
      ) : null}
    </div>
  );
};

export default ContentItem;