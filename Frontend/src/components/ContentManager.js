//работа с контентом (текст/видео/ссылки)

import React, { useState } from 'react';

const ContentManager = () => {
  const [content, setContent] = useState([]);
  const [form, setForm] = useState({ type: 'text', title: '', value: '' });

  const handleChange = e => setForm({ ...form, [e.target.name]: e.target.value });

  const handleAdd = () => {
    setContent([...content, { ...form, id: Date.now() }]);
    setForm({ type: 'text', title: '', value: '' });
  };

  const handleDelete = id => setContent(content.filter(c => c.id !== id));

  return (
    <div>
      <h2 className="text-xl font-bold mb-2">Управление материалами</h2>
      <div className="space-y-2">
        <select name="type" value={form.type} onChange={handleChange} className="border p-1">
          <option value="text">Текст</option>
          <option value="video">Видео</option>
          <option value="link">Ссылка</option>
        </select>
        <input
          name="title"
          placeholder="Заголовок"
          value={form.title}
          onChange={handleChange}
          className="border p-1 w-full"
        />
        <input
          name="value"
          placeholder="Контент / ссылка"
          value={form.value}
          onChange={handleChange}
          className="border p-1 w-full"
        />
        <button onClick={handleAdd} className="bg-blue-500 text-white px-3 py-1 rounded">Добавить</button>
      </div>
      <ul className="mt-4">
        {content.map(item => (
          <li key={item.id} className="border-b py-2 flex justify-between">
            <div>
              <strong>{item.title}</strong> ({item.type}) — {item.value}
            </div>
            <button onClick={() => handleDelete(item.id)} className="text-red-500">Удалить</button>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default ContentManager;