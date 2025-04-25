import React, { useEffect, useState, useCallback } from 'react';
import { useParams } from 'react-router-dom';
import {
  getContentItemsByTopic,
  getContentItemById,
  deleteContentItem,
  createWordContentItem,
  createImageContentItem,
  createBookContentItem,
  createAudioContentItem,
} from '../api/contentItemService';

const ContentItemPage = () => {
  const { topicId } = useParams(); // topicId, а не id
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

  const [lookupId, setLookupId] = useState('');
  const [lookedUpItem, setLookedUpItem] = useState(null);
  const [loading, setLoading] = useState(true); // добавляем состояние для загрузки

  // loadContentItems теперь использует topicId
  const loadContentItems = useCallback(async () => {
    try {
      setLoading(true); // включаем индикатор загрузки
      const data = await getContentItemsByTopic(topicId); // используй topicId вместо id
      setItems(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false); // выключаем индикатор загрузки
    }
  }, [topicId]); // добавляем topicId в зависимости

  useEffect(() => {
    loadContentItems();
  }, [loadContentItems]);

  const handleDelete = async (id) => {
    try {
      await deleteContentItem(id);
      loadContentItems(); // обновляем список после удаления
    } catch (err) {
      console.error(err);
    }
  };

  const handleChange = (e) => {
    setFormData((prev) => ({ ...prev, [e.target.name]: e.target.value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      switch (type) {
        case 'word':
          await createWordContentItem({
            word: formData.word,
            translation: formData.translation,
            topicId,
          });
          break;
        case 'image':
          await createImageContentItem({
            imageUrl: formData.imageUrl,
            translation: formData.translation,
            topicId,
          });
          break;
        case 'book':
          await createBookContentItem({
            title: formData.bookTitle,
            author: formData.bookAuthor,
            topicId,
          });
          break;
        case 'audio':
          await createAudioContentItem({
            audioUrl: formData.audioUrl,
            translation: formData.translation,
            topicId,
          });
          break;
        default:
          return;
      }

      // сбрасываем форму после добавления
      setFormData({
        word: '',
        imageUrl: '',
        bookTitle: '',
        bookAuthor: '',
        audioUrl: '',
        translation: '',
      });

      loadContentItems(); // обновляем список контента
    } catch (err) {
      console.error(err);
    }
  };

  const handleLookup = async () => {
    try {
      const item = await getContentItemById(lookupId);
      setLookedUpItem(item);
    } catch (err) {
      console.error('Error fetching by ID:', err);
      setLookedUpItem(null);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h2>Content Items for Topic ID: {topicId}</h2>

      <form onSubmit={handleSubmit} style={{ marginBottom: '30px' }}>
        <label>
          Type:
          <select value={type} onChange={(e) => setType(e.target.value)}>
            <option value="word">Word</option>
            <option value="image">Image</option>
            <option value="book">Book</option>
            <option value="audio">Audio</option>
          </select>
        </label>

        {type === 'word' && (
          <>
            <input
              name="word"
              placeholder="Word"
              value={formData.word}
              onChange={handleChange}
            />
            <input
              name="translation"
              placeholder="Translation"
              value={formData.translation}
              onChange={handleChange}
            />
          </>
        )}

        {type === 'image' && (
          <>
            <input
              name="imageUrl"
              placeholder="Image URL"
              value={formData.imageUrl}
              onChange={handleChange}
            />
            <input
              name="translation"
              placeholder="Translation"
              value={formData.translation}
              onChange={handleChange}
            />
          </>
        )}

        {type === 'book' && (
          <>
            <input
              name="bookTitle"
              placeholder="Book Title"
              value={formData.bookTitle}
              onChange={handleChange}
            />
            <input
              name="bookAuthor"
              placeholder="Author"
              value={formData.bookAuthor}
              onChange={handleChange}
            />
          </>
        )}

        {type === 'audio' && (
          <>
            <input
              name="audioUrl"
              placeholder="Audio URL"
              value={formData.audioUrl}
              onChange={handleChange}
            />
            <input
              name="translation"
              placeholder="Translation"
              value={formData.translation}
              onChange={handleChange}
            />
          </>
        )}

        <button type="submit">Create</button>
      </form>

      <div style={{ marginBottom: '30px' }}>
        <h3>Get Content Item by ID</h3>
        <input
          type="text"
          placeholder="Enter ID"
          value={lookupId}
          onChange={(e) => setLookupId(e.target.value)}
        />
        <button onClick={handleLookup}>Lookup</button>

        {lookedUpItem && (
          <pre style={{ backgroundColor: '#f1f1f1', padding: '10px' }}>
            {JSON.stringify(lookedUpItem, null, 2)}
          </pre>
        )}
      </div>

      <h3>All Items</h3>
      {loading ? (
        <p>Loading...</p>
      ) : (
        <ul>
          {items.map((item) => (
            <li key={item.id} style={{ marginBottom: '10px' }}>
              <pre>{JSON.stringify(item, null, 2)}</pre>
              <button onClick={() => handleDelete(item.id)}>Delete</button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default ContentItemPage;