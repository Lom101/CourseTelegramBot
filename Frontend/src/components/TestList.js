import React from 'react';
import { Link } from "react-router-dom";

const TestList = ({ tests }) => (
  <div className="mt-8">
    <h2 className="text-2xl mb-4 text-gray-800">Список существующих тестов</h2>
    {tests.length > 0 ? (
      tests.map((test) => (
        <div key={test.id} className="border p-4 rounded-lg mb-4 flex justify-between items-center shadow-md bg-white">
          <Link to={`/test/${test.id}`} className="text-xl text-blue-600 font-semibold hover:underline">
            {test.title}
          </Link>
          <Link to={`/tests/edit/${test.id}`} className="bg-yellow-400 px-4 py-2 rounded text-white hover:bg-yellow-500 transition-colors duration-200">
            Редактировать
          </Link>
        </div>
      ))
    ) : (
      <p className="text-lg text-gray-600">Нет доступных тестов.</p>
    )}
  </div>
);

export default TestList;