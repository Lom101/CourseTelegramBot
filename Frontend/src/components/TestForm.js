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
      className="bg-green-500 text-white p-2 w-48 mt-2 rounded-lg hover:bg-green-600"
      onClick={handleCreateTest}
    >
      Создать тест
    </button>
  </div>
);

export default TestForm;