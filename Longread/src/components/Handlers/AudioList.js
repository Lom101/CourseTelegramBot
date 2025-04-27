import React from 'react';
import { BASE_URL } from '../../pages/LongreadPage';

const AudioList = ({ audioFiles }) => (
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
);

export default AudioList;