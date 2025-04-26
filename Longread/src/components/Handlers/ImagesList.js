import React from 'react';
import { BASE_URL } from '../LongreadPage/LongreadPage';

const ImagesList = ({ images }) => (
  <div className="images">
    {images.map(image => (
      <div key={image.id} className="image-item">
        <h3>{image.title}</h3>
        <img 
          src={`${BASE_URL}${image.fileUrl}`} 
          alt={image.altText || 'Изображение'} 
          className="image"
        />
      </div>
    ))}
  </div>
);

export default ImagesList;