import React from 'react';

const icons = {
  text: "📄",
  picture: "🖼️",
  audio: "🎵",
  book: "📘"
};

const ContentItem = ({ type, title, path }) => {
  if (type === 'picture') {
    return (
      <div className="border rounded-lg p-3 shadow bg-white">
        <p className="font-semibold mb-2">{icons.picture} {title}</p>
        <a href={path} target="_blank" rel="noopener noreferrer">
          <img
            src={path}
            alt={title}
            className="w-full h-auto max-h-60 object-contain rounded"
            onError={(e) => (e.target.style.display = 'none')}
          />
        </a>
      </div>
    );
  }

  if (type === 'audio') {
    return (
      <div className="border rounded-lg p-3 shadow bg-white">
        <p className="font-semibold mb-2">{icons.audio} {title}</p>
        <audio controls className="w-full">
          <source src={path} />
          Your browser does not support the audio element.
        </audio>
      </div>
    );
  }

  return (
    <div className="border rounded-lg p-3 shadow bg-white">
      <p className="font-semibold mb-1">{icons[type]} {title}</p>
      <a
        href={path}
        target="_blank"
        rel="noopener noreferrer"
        className="text-blue-600 hover:underline"
      >
        {title}
      </a>
    </div>
  );
};

export default ContentItem;