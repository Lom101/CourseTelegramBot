import React from 'react';

const WordContent = ({ htmlContent }) => (
  <div className="longread-content" dangerouslySetInnerHTML={{ __html: htmlContent }} />
);

export default WordContent;