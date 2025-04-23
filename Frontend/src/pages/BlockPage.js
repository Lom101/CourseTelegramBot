import React from 'react';
import { useParams, Link } from 'react-router-dom';
import ContentManager from '../components/ContentManager';

const BlockPage = () => {
  const { id } = useParams();

  return (
    <div>
      <Link
        to="/materials"
        className="inline-flex items-center gap-2 text-[#2E2E2E] hover:text-white bg-[#E2E8F0] hover:bg-[#A6C5E2] transition-colors px-4 py-2 rounded-xl text-sm font-medium w-fit mb-6"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          className="h-4 w-4"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          strokeWidth="2"
        >
          <path strokeLinecap="round" strokeLinejoin="round" d="M15 19l-7-7 7-7" />
        </svg>
        Назад
      </Link>
  
      <div className="mb-6">
        <h2 className="relative inline-block text-white text-xl md:text-2xl font-semibold">
          <span className="bg-[#8BC34A] py-2 px-4 pr-10 rounded-r-[30px] skew-x-[-10deg] inline-block">
            <span className="skew-x-[10deg] block">Материалы блока {id}</span>
          </span>
        </h2>
      </div>
  
      <ContentManager blockId={id} />
    </div>
  );
};

export default BlockPage;