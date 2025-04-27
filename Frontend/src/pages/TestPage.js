import React, { useEffect, useState } from 'react';
import { Link } from "react-router-dom";
import { getAllTests, createTest } from '../api/testService'; // Функции для получения и добавления тестов

const TestPage = () => {
  const [tests, setTests] = useState([]);
  const [newTestTitle, setNewTestTitle] = useState('');
  const [newTestQuestions, setNewTestQuestions] = useState([{ questionText: '', options: [{ optionText: '' }] }]); // Пример структуры для вопросов

  // Функция для получения всех тестов
  useEffect(() => {
    const fetchTests = async () => {
      try {
        const data = await getAllTests();
        setTests(data);
      } catch (error) {
        console.error('Ошибка при получении тестов:', error);
      }
    };

    fetchTests();
  }, []);

  // Функция для создания нового теста
  const handleCreateTest = async () => {
    const newTest = {
      title: newTestTitle,
      questions: newTestQuestions,
    };

    try {
      await createTest(newTest);
      setNewTestTitle('');
      setNewTestQuestions([{ questionText: '', options: [{ optionText: '' }] }]);
      // После создания нового теста обновляем список тестов
      const data = await getAllTests();
      setTests(data);
    } catch (error) {
      console.error('Ошибка при создании теста:', error);
    }
  };

  // Функция для добавления вопроса
  const handleAddQuestion = () => {
    setNewTestQuestions([
      ...newTestQuestions,
      { questionText: '', options: [{ optionText: '' }] },
    ]);
  };

  // Функция для добавления варианта ответа
  const handleAddOption = (index) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[index].options.push({ optionText: '' });
    setNewTestQuestions(updatedQuestions);
  };

  // Функция для обновления текста вопроса
  const handleQuestionChange = (index, value) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[index].questionText = value;
    setNewTestQuestions(updatedQuestions);
  };

  // Функция для обновления текста варианта ответа
  const handleOptionChange = (qIndex, oIndex, value) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[qIndex].options[oIndex].optionText = value;
    setNewTestQuestions(updatedQuestions);
  };

  return (
    <div className="m-10">
      <h1 className="text-2xl mb-4">Список тестов</h1>
  
      {/* Добавление нового теста */}
      <div className="mb-8 p-6 rounded-lg shadow-md bg-white">
        <h2 className="text-2xl mb-4 text-gray-800">Создать новый тест</h2>
        <input
          type="text"
          value={newTestTitle}
          onChange={(e) => setNewTestTitle(e.target.value)}
          placeholder="Введите название теста"
          className="border p-3 w-full mb-6 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
  
        <div className="mb-6 w-full">
          <h3 className="text-xl mb-4 text-gray-700">Вопросы</h3>
          {newTestQuestions.map((question, qIndex) => (
            <div key={qIndex} className="mb-6 w-full">
              <input
                type="text"
                value={question.questionText}
                onChange={(e) => handleQuestionChange(qIndex, e.target.value)}
                placeholder={`Вопрос ${qIndex + 1}`}
                className="border p-3 w-full mb-4 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
              <div className="space-y-4">
                {question.options.map((option, oIndex) => (
                  <input
                    key={oIndex}
                    type="text"
                    value={option.optionText}
                    onChange={(e) => handleOptionChange(qIndex, oIndex, e.target.value)}
                    placeholder={`Вариант ${oIndex + 1}`}
                    className="border p-3 w-full rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                  />
                ))}
                <button
                  className="bg-blue-500 text-white p-2 rounded w-48 h-12 hover:bg-blue-600"
                  onClick={() => handleAddOption(qIndex)}
                >
                  Добавить вариант
                </button>
              </div>
            </div>
          ))}
          <button
            className="bg-green-500 text-white p-2 rounded w-48 h-12 hover:bg-green-600 mt-2"
            onClick={handleAddQuestion}
          >
            Добавить вопрос
          </button>
        </div>
  
        <button
          className="bg-green-600 text-white p-2 rounded w-48 h-12 hover:bg-green-700 mt-3"
          onClick={handleCreateTest}
        >
          Создать тест
        </button>
      </div>
  
      {/* Список тестов */}
      <div className="mt-8">
        <h2 className="text-2xl mb-4 text-gray-800">Список существующих тестов</h2>
        {tests.length > 0 ? (
          tests.map((test) => (
            <div
              key={test.id}
              className="border p-4 rounded-lg mb-4 flex justify-between items-center shadow-md bg-white"
            >
              <Link
                to={`/test/${test.id}`}
                className="text-xl text-blue-600 font-semibold hover:underline"
              >
                {test.title}
              </Link>
              <Link
                to={`/tests/edit/${test.id}`}
                className="bg-yellow-400 px-4 py-2 rounded text-white hover:bg-yellow-500 transition-colors duration-200"
              >
                Редактировать
              </Link>
            </div>
          ))
        ) : (
          <p className="text-lg text-gray-600">Нет доступных тестов.</p>
        )}
      </div>
    </div>
  );
};

export default TestPage;
