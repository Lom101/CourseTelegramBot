import React from 'react';
import { Link } from "react-router-dom";

const TestList = ({ tests }) => (
  <div className="mt-8">
    <h2 className="text-4xl font-extrabold text-gray-800 mb-2">Список существующих тестов</h2>
    {tests.length > 0 ? (
      tests.map((test) => (
        <div key={test.id} className="border p-4 rounded-lg mb-4 flex justify-between items-center shadow-md bg-white">
          <Link to={`/test/${test.id}`} className="text-xl text-gray-600 font-semibold hover:underline">
            {test.title}
          </Link>
          <Link to={`/tests/edit/${test.id}`} className="text-xl font-bold mb-4 text-gray-700">
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