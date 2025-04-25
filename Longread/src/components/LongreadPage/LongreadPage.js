import React, { useEffect, useState } from 'react';
import mammoth from 'mammoth';
import { useParams } from 'react-router-dom';
import './LongreadPage.css';

export const BASE_URL = 'http://localhost:5000';

const LongreadPage = () => {
  const { topicId } = useParams(); // Получаем параметр из URL
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
        // заголовок страницы
        const blockResponse = await fetch(` ${BASE_URL}/api/Block/4` ); 
        const blockData = await blockResponse.json();
        setPageTitle(blockData.title);

        //информацию о теме
        const topicResponse = await fetch(` ${BASE_URL}/api/Topic/4` );
        const topicData = await topicResponse.json();
        setTopicTitle(topicData.title);

        // Получаем и конвертируем DOCX файл
        const response = await fetch(` http://localhost:5000/api/ContentItem/by-topic/4` );
        const data = await response.json();

        // Находим DOCX файл для конвертации
        const docxItem = data.find(item => item.fileUrl && item.fileUrl.endsWith('.docx'));

        if (!docxItem) {
          setError('DOCX файл не найден');
          return;
        }

        const fullDocxUrl = ` ${BASE_URL}${docxItem.fileUrl}` ;
        const fileResponse = await fetch(fullDocxUrl);
        const arrayBuffer = await fileResponse.arrayBuffer();

        const result = await mammoth.convertToHtml({ arrayBuffer });
        setHtmlContent(result.value);

        // книги с PDF
        const books = data.filter(item => item.fileUrl && item.fileUrl.endsWith('.pdf'));
        setBooks(books);

        // аудио файлы
        const audioFiles = data.filter(item => item.audioUrl);
        setAudioFiles(audioFiles);

        // изображения
        const images = data.filter(item => item.fileUrl && (item.fileUrl.endsWith('.jpg') || item.fileUrl.endsWith('.png')));
        setImages(images);

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

  const scrollToBooks = () => {
    const booksSection = document.getElementById('books-section');
    if (booksSection) {
      booksSection.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <div className="longread-page">
      <div className="header">
        <img src="/content/logo192.jpg" alt="Компания" className="company-logo" />
        <h1 className="page-title">{pageTitle}</h1>
      </div>

      <div className="content">
        <h2 className="header">{topicTitle}</h2>
        <div className="longread-content" dangerouslySetInnerHTML={{ __html: htmlContent }} />


        {/* Книги */}
        <div className="books" id="books-section">
          {books.map(book => (
            <div key={book.id} className="book-item">
              <h3>{book.title}</h3>
              <a 
                href={` ${BASE_URL}${book.fileUrl}` } 
                download={book.fileName} 
                className="download-btn"
              >
                Скачать книгу
              </a>
            </div>
          ))}
        </div>
        {/* Аудио файлы */}
        <div className="audio-files">
  {audioFiles.map(audio => (
    <div key={audio.id} className="audio-item">
      <h3>{audio.title}</h3>
      <audio controls>
      <source src={`${BASE_URL}${encodeURI(audio.audioUrl)}`} type="audio/mp4" />
        Ваш браузер не поддерживает элемент audio.
      </audio>
    </div>
  ))}
</div>

        {/* Изображения */}
        <div className="images">
          {images.map(image => (
            <div key={image.id} className="image-item">
              <h3>{image.title}</h3>
              <img 
                src={` ${BASE_URL}${image.fileUrl}` } 
                alt={image.altText || 'Изображение'} 
                className="image"
              />
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default LongreadPage;