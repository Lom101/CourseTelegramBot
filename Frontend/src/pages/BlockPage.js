import React from 'react';
import { useParams, Link } from 'react-router-dom';
import ContentManager from '../components/ContentManager';

const BlockPage = () => {
  const { blockId } = useParams();

  return (
    <div>
      <h1>Материалы {blockId}</h1>
      <Link to="/" style={{ marginBottom: '1rem', display: 'inline-block' }}>← Назад</Link>
      <ContentManager blockId={blockId} />
    </div>
  );
};

export default BlockPage;