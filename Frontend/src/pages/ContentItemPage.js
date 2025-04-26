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
    word: '',
    imageUrl: '',
    bookTitle: '',
    bookAuthor: '',
    audioUrl: '',
    translation: '',
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
    try{
      try {
        const topic = await getTopicById(topicId); // Получаем тему по ID
      }
      catch {
        console.error('Topic not found');
        navigate('/notfound');  // Если тема не найдена, редиректим на страницу notfound
      } 
      loadContentItems();  // Если тема найдена, загружаем элементы
    }
      catch (err) {
      console.error('Error fetching content items:', err);  // Ошибка при загрузке контента
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
    setFormData((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

const handleSubmit = async (e) => {
  e.preventDefault();

  const contentData = {
    word: formData.word,
    translation: formData.translation,
    imageUrl: formData.imageUrl,
    bookTitle: formData.bookTitle,
    bookAuthor: formData.bookAuthor,
    audioUrl: formData.audioUrl,
    topicId,
  };

  const createFunctions = {
    word: createWordContentItem,
    image: createImageContentItem,
    book: createBookContentItem,
    audio: createAudioContentItem,
  };

  try {
    await createFunctions[type](contentData);
    setFormData({
      word: '',
      imageUrl: '',
      bookTitle: '',
      bookAuthor: '',
      audioUrl: '',
      translation: '',
    });
    loadContentItems();
  } catch (err) {
    console.error(err);
  }
};

return (
  <div className="m-10">

      <h2 className="text-2xl font-semibold mb-4">Элементы longread-сайта внутри темы №{topicId}</h2>

       {/* Create Content Item */}
      <div className="relative">
        <form onSubmit={handleSubmit} className="space-y-4 mb-6">
          <div className="flex items-center space-x-4">
            <label className="text-lg">Type:</label>
            <select
              value={type}
              onChange={(e) => setType(e.target.value)}
              className="border p-2 rounded"
            >
              <option value="word">Word</option>
              <option value="image">Image</option>
              <option value="book">Book</option>
              <option value="audio">Audio</option>
            </select>
          </div>

          {type === 'word' && (
            <>
              <div>
                <input
                  name="word"
                  placeholder="Word"
                  value={formData.word}
                  onChange={handleChange}
                  className="w-full w-auto p-2 border rounded"
                />
              </div>
              <div>
                <input
                  name="translation"
                  placeholder="Translation"
                  value={formData.translation}
                  onChange={handleChange}
                  className="w-full w-auto p-2 border rounded"
                />
              </div>
            </>
          )}

          {type === 'image' && (
            <>
              <div>
                <input
                  name="imageUrl"
                  placeholder="Image URL"
                  value={formData.imageUrl}
                  onChange={handleChange}
                  className="w-full w-auto p-2 border rounded"
                />
              </div>
              <div>
                <input
                  name="translation"
                  placeholder="Translation"
                  value={formData.translation}
                  onChange={handleChange}
                  className="w-full w-auto p-2 border rounded"
                />
              </div>
            </>
          )}

          {type === 'book' && (
            <>
              <div>
                <input
                  name="bookTitle"
                  placeholder="Book Title"
                  value={formData.bookTitle}
                  onChange={handleChange}
                  className="w-full w-auto  p-2 border rounded"
                />
              </div>
              <div>
                <input
                  name="bookAuthor"
                  placeholder="Author"
                  value={formData.bookAuthor}
                  onChange={handleChange}
                  className="w-full w-auto  p-2 border rounded"
                />
              </div>
            </>
          )}

          {type === 'audio' && (
            <>
              <div>
                <input
                  name="audioUrl"
                  placeholder="Audio URL"
                  value={formData.audioUrl}
                  onChange={handleChange}
                  className="w-full w-auto  p-2 border rounded"
                />
              </div>
              <div>
                <input
                  name="translation"
                  placeholder="Translation"
                  value={formData.translation}
                  onChange={handleChange}
                  className="w-full w-auto  p-2 border rounded"
                />
              </div>
            </>
          )}

          <button type="submit" className="w-auto p-5 py-2 bg-blue-500 text-white rounded hover:bg-blue-600">
            Create
          </button>
        </form>
      </div>

      <h3 className="text-xl font-semibold mb-4">All Items</h3>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {items.map((item) => (
            <div
              key={item.id}
              className="bg-white p-4 border rounded shadow-md flex flex-col gap-4"
            >
              <h4 className="text-lg font-semibold">{item.title || item.fileName}</h4>
              {item.fileUrl && (
                <a
                  href={process.env.REACT_APP_API_URL + item.fileUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-blue-500 hover:underline"
                >
                  View File
                </a>
              )}
              <button
                onClick={() => handleDelete(item.id)}
                className="self-start bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
              >
                Delete
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default ContentItemPage;
