import React from 'react';
import '../index.css';

const ContentItem = ({type, title, path}) => {
    const renderContent = () => {
        switch (type) {
            case 'audio':
                return <audio controls src={path} className="content-audio" />;
            case 'book':
                return (
                <a href={path} target="_blank" rel="noopener noreferrer" className='content-link'>
                    📝{title}
                </a>
                );
            case 'picture':
                return <img src={path} alt={title} className="content-audio" />;
            case 'text':
                return (
                <a href={path} target='_blank' rel='nooper noreferrer' className='content-link'>
                    📝{title}
                </a>
                );
            default:
                return null;
        }
    };

    return (
        <div className='content-item'>
            <h4>{title}</h4>
            {renderContent()}
        </div>
    );
};

export default ContentItem;