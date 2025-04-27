import React, { useEffect, useState } from 'react';
import mammoth from 'mammoth';
import { useParams } from 'react-router-dom';
import './LongreadPage.css';

import WordContent from '../components/Handlers/WordContent';
import BooksList from '../components/Handlers/BooksList';
import AudioList from '../components/Handlers/AudioList';
import ImagesList from '../components/Handlers/ImagesList';

export const BASE_URL = 'http://localhost:5000';

const LongreadPage = () => {
  const { topicId } = useParams();
  const [htmlContent, setHtmlContent] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [books, setBooks] = useState([]);
  const [audioFiles, setAudioFiles] = useState([]);
  const [images, setImages] = useState([]);
  const [pageTitle, setPageTitle] = useState('');
  const [topicTitle, setTopicTitle] = useState('');

  useEffect(() => {
    const fetchAndConvertDocx = async () => {
      try {
        if (!topicId) {
          setError('Topic ID не найден в URL');
          setLoading(false);
          return;
        }
        const topicResponse = await fetch(`${BASE_URL}/api/Topic/${topicId}`);
        const topicData = await topicResponse.json();
        setTopicTitle(topicData.title);

        const response = await fetch(`${BASE_URL}/api/ContentItem/by-topic/${topicId}`);
        const data = await response.json();

        const docxItem = data.find(item => item.fileUrl && item.fileUrl.endsWith('.docx'));
        if (docxItem) {
          const fullDocxUrl = `${BASE_URL}${docxItem.fileUrl}`;
          const fileResponse = await fetch(fullDocxUrl);
          const arrayBuffer = await fileResponse.arrayBuffer();
          const result = await mammoth.convertToHtml({ arrayBuffer });
          setHtmlContent(result.value);
        }

        setBooks(data.filter(item => item.fileUrl && item.fileUrl.endsWith('.pdf')));
        setAudioFiles(data.filter(item => item.audioUrl));
        setImages(data.filter(item => item.fileUrl && (item.fileUrl.endsWith('.jpg') || item.fileUrl.endsWith('.png'))));

      } catch (err) {
        console.error('Ошибка:', err);
        setError('Ошибка при загрузке данных');
      } finally {
        setLoading(false);
      }
    };

    fetchAndConvertDocx();
  }, [topicId]);

  if (loading) return <p>Загрузка...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="longread-page">
      <div className="header">
        <img src="/content/logo192.jpg" alt="Компания" className="company-logo" />
        <h1 className="page-title">{pageTitle}</h1>
      </div>

      <div className="content">
        <h2>{topicTitle}</h2>

        {htmlContent && <WordContent htmlContent={htmlContent} />}
        {books.length > 0 && <BooksList books={books} />}
        {audioFiles.length > 0 && <AudioList audioFiles={audioFiles} />}
        {images.length > 0 && <ImagesList images={images} />}
      </div>
    </div>
  );
};

export default LongreadPage;