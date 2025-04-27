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
    <div className="m-10">
      <h2 className="text-2xl font-semibold mb-4">Содержимое темы №{topicId}</h2>

      {/* Create Content Item */}
      <form onSubmit={handleSubmit} className="space-y-4 mb-6" encType="multipart/form-data">
        <div className="flex items-center space-x-4">
          <label className="text-lg">Тип</label>
          <select
            value={type}
            onChange={(e) => setType(e.target.value)}
            className="border p-2 rounded"
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
            placeholder="Title"
            value={formData.title}
            onChange={handleChange}
            className="w-full p-2 border rounded"
          />
        </div>

        {(type === 'image') && (
          <div>
            <input
              name="altText"
              placeholder="Alt Text (optional)"
              value={formData.altText}
              onChange={handleChange}
              className="w-full p-2 border rounded"
            />
          </div>
        )}

        <div>
          <input
            type="file"
            name="file"
            accept={type === 'audio' ? 'audio/*' : type === 'image' ? 'image/*' : ''}
            onChange={handleChange}
            className="w-full p-2 border rounded"
          />
        </div>

        <button type="submit" className="w-auto p-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
          Добавить
        </button>
      </form>

      <h3 className="text-xl font-semibold mb-4">Все элементы</h3>
      {loading ? (
        <p>Загрузка...</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {items.map((item) => (
            <div
              key={item.id}
              className="bg-white p-4 border rounded shadow-md flex flex-col gap-4"
            >
              <h4 className="text-lg font-semibold">{item.title || 'Без названия'}</h4>

              {item.fileUrl && (
                <a
                  href={process.env.REACT_APP_API_URL + item.fileUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
                >
                  Посмотреть файл
                </a>
              )}

              <button
                onClick={() => handleDelete(item.id)}
                className="self-start bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
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
