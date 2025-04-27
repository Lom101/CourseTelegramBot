import React from 'react';

const TestForm = ({ newTestTitle, setNewTestTitle, handleCreateTest }) => (
  <div className="mb-4 space-x-4">
    <input
      type="text"
      value={newTestTitle}
      onChange={(e) => setNewTestTitle(e.target.value)}
      placeholder="Введите название теста"
      className="border p-2 w-60 mb-4 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
    />
    <button
      className="px-6 py-2 bg-gray-700 hover:bg-gray-800 text-white font-semibold rounded-xl transition h-fit"
      onClick={handleCreateTest}
    >
      Создать тест
    </button>
  </div>
);

export default TestForm;