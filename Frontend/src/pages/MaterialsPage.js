import React from 'react';
import { Link } from 'react-router-dom';

const blocks = [1, 2, 3, 4];

const MaterialsPage = () => {
  return (
    <div>
      <div className="mb-6">
        <h2 className="text-4xl font-extrabold bg-gradient-to-r from-[#ffcc00] via-[#ffb703] to-[#ff9800] text-transparent bg-clip-text drop-shadow-md tracking-tight flex items-center gap-3">
          <span className="text-5xl">Всего</span> блоков в курсе: {blocks.length}
        </h2>
        <div className="h-1 w-24 bg-gradient-to-r from-[#ffcc00] to-[#ff9800] rounded-full mt-2" />
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-4">
        {blocks.map((blockId) => (
          <Link
            key={blockId}
            to={`/materials/block/${blockId}`}
            className="rounded-2xl border border-[#EDEDED] p-4 bg-white text-[#2E2E2E] shadow hover:shadow-md hover:bg-[#D4F4DD] transition"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
              strokeLinejoin="round"
              className="mb-2"
            >
              <path stroke="none" d="M0 0h24v24H0z" fill="none" />
              <path d="M12 3l8 4.5l0 9l-8 4.5l-8 -4.5l0 -9l8 -4.5" />
              <path d="M12 12l8 -4.5" />
              <path d="M12 12l0 9" />
              <path d="M12 12l-8 -4.5" />
            </svg>
            Блок {blockId}
          </Link>
        ))}

        <button
          onClick={() => alert("Добавить блок?")}
          className="flex items-center gap-2 border rounded-2xl px-6 py-3 bg-[#B4FCFD] text-[#2E2E2E] hover:bg-[#87CEEB] transition shadow"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="20"
            height="20"
            viewBox="0 0 24 24"
            fill="none"
            stroke="currentColor"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          >
            <path stroke="none" d="M0 0h24v24H0z" fill="none" />
            <path d="M21 12.5v-4.509a1.98 1.98 0 0 0 -1 -1.717l-7 -4.008a2.016 2.016 0 0 0 -2 0l-7 4.007c-.619 .355 -1 1.01 -1 1.718v8.018c0 .709 .381 1.363 1 1.717l7 4.008a2.016 2.016 0 0 0 2 0" />
            <path d="M12 22v-10" />
            <path d="M12 12l8.73 -5.04" />
            <path d="M3.27 6.96l8.73 5.04" />
            <path d="M16 19h6" />
            <path d="M19 16v6" />
          </svg>
          <span>Добавить блок</span>
        </button>
      </div>
    </div>
  );
};

export default MaterialsPage;