import React from 'react';
import { BASE_URL } from '../../pages/LongreadPage';

const BooksList = ({ books }) => (
  <div className="books" id="books-section">
    {books.map(book => (
      <div key={book.id} className="book-item">
        <h3>{book.title}</h3>
        <a 
          href={`${BASE_URL}${book.fileUrl}`} 
          download={book.fileName} 
          className="download-btn"
        >
          Скачать книгу
        </a>
      </div>
    ))}
  </div>
);

export default BooksList;