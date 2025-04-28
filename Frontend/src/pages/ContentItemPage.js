import React, { useEffect, useState, useCallback } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  getContentItemsByTopic,
  deleteContentItem,
  createWordContentItem,
  createImageContentItem,
  createBookContentItem,
  createAudioContentItem,
} from '../api/contentItemService';
import { getTopicById } from '../api/topicService';

const ContentItemPage = () => {
  const { topicId } = useParams();
  const navigate = useNavigate();

  const [items, setItems] = useState([]);
  const [type, setType] = useState('word');
  const [formData, setFormData] = useState({
    title: '',
    altText: '',
    file: null,
  });

  const [loading, setLoading] = useState(true);

  const loadContentItems = useCallback(async () => {
    try {
      setLoading(true);
      const data = await getContentItemsByTopic(topicId);
      setItems(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [topicId]);

  const checkTopicExistence = async () => {
    try {
      await getTopicById(topicId);
      loadContentItems();
    } catch {
      console.error('Topic not found');
      navigate('/notfound');
    }
  };

  useEffect(() => {
    checkTopicExistence();
  }, [topicId]);

  const handleDelete = async (id) => {
    try {
      await deleteContentItem(id);
      loadContentItems();
    } catch (err) {
      console.error(err);
    }
  };

  const handleChange = (e) => {
    const { name, value, files } = e.target;
    if (files) {
      setFormData((prev) => ({ ...prev, file: files[0] }));
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formDataToSend = new FormData();
    formDataToSend.append('TopicId', topicId);
    formDataToSend.append('Title', formData.title);
    if (formData.file) {
      const fileFieldName = type === 'image' ? 'Image' : type === 'audio' ? 'AudioFile' : 'File';
      formDataToSend.append(fileFieldName, formData.file);
    }
    if (type === 'image') {
      formDataToSend.append('AltText', formData.altText);
    }

    const createFunctions = {
      word: createWordContentItem,
      image: createImageContentItem,
      book: createBookContentItem,
      audio: createAudioContentItem,
    };

    try {
      await createFunctions[type](formDataToSend);
      setFormData({ title: '', altText: '', file: null });
      loadContentItems();
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="m-10 bg-gray-100 p-8 rounded-lg shadow">
    <h2 className="text-2xl font-extrabold text-gray-800 mb-6">Содержимое темы №{topicId}</h2>
  
    {/* Create Content Item */}
    <form onSubmit={handleSubmit} className="space-y-6 mb-8" encType="multipart/form-data">
      <div className="flex items-center space-x-4">
        <label className="text-lg font-medium text-gray-700">Тип</label>
        <select
          value={type}
          onChange={(e) => setType(e.target.value)}
          className="border border-gray-300 p-3 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-gray-400"
        >
          <option value="word">Word файл</option>
          <option value="image">Картинка</option>
          <option value="book">Книга</option>
          <option value="audio">Аудио файл</option>
        </select>
      </div>
  
      <div>
        <input
          name="title"
          placeholder="Название"
          value={formData.title}
          onChange={handleChange}
          className="w-full p-3 border border-gray-300 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-gray-400"
        />
      </div>
  
      {type === 'image' && (
        <div>
          <input
            name="altText"
            placeholder="Альтернативный текст (опционально)"
            value={formData.altText}
            onChange={handleChange}
            className="w-full p-3 border border-gray-300 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-gray-400"
          />
        </div>
      )}
  
      <div>
        <input
          type="file"
          name="file"
          accept={type === 'audio' ? 'audio/*' : type === 'image' ? 'image/*' : ''}
          onChange={handleChange}
          className="w-full p-3 border border-gray-300 rounded-lg bg-white focus:outline-none focus:ring-2 focus:ring-gray-400"
        />
      </div>
  
      <button
        type="submit"
        className="w-full p-3 bg-gray-500 text-white rounded-lg hover:bg-gray-600 transition"
      >
        Добавить
      </button>
    </form>
  
    <h3 className="text-xl font-bold text-gray-800 mb-6">Все элементы</h3>
  
    {loading ? (
      <p className="text-gray-600">Загрузка...</p>
    ) : (
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {items.map((item) => (
          <div
            key={item.id}
            className="bg-white p-6 border border-gray-300 rounded-lg shadow-sm flex flex-col gap-4 hover:shadow-md transition"
          >
            <h4 className="text-lg font-semibold text-gray-700">{item.title || 'Без названия'}</h4>
  
            {item.fileUrl && (
              <a
                href={process.env.REACT_APP_API_URL + item.fileUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="text-gray-600 hover:text-gray-800 underline"
              >
                Посмотреть файл
              </a>
            )}
  
            <button
              onClick={() => handleDelete(item.id)}
              className="self-start bg-red-400 text-white px-4 py-2 rounded-lg hover:bg-red-500 transition"
            >
              Удалить
            </button>
          </div>
        ))}
      </div>
    )}
  </div>
  
  );
};

export default ContentItemPage;
