import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom'; // Импортируем useNavigate
import { getTestById } from '../api/testService'; // Функция для получения теста по ID

const TestDetailPage = () => {
  const { id } = useParams(); // Получаем ID теста из URL
  const [test, setTest] = useState(null);
  const navigate = useNavigate(); // Инициализация useNavigate для переходов

  // Загружаем тест по ID
  useEffect(() => {
    const fetchTest = async () => {
      try {
        const data = await getTestById(id);
        setTest(data);
      } catch (error) {
        console.error('Ошибка при получении теста:', error);
      }
    };

    fetchTest();
  }, [id]);

  if (!test) return <div>Загрузка...</div>;

  return (
    <div className="mb-8 p-6 rounded-lg shadow-md bg-white">
      <h1 className="text-3xl mb-6">{test.title}</h1>

      {/* Выводим вопросы и варианты */}
      {test.questions.map((question, qIndex) => (
        <div key={qIndex} className="mb-6">
          <h2 className="text-xl mb-4">{`Вопрос ${qIndex + 1}: ${question.questionText}`}</h2>
          <ul className="space-y-2">
            {question.options.map((option, oIndex) => (
              <li key={oIndex} className="flex items-center">
                <span className="ml-2">{`${oIndex + 1}. ${option.optionText}`}</span>
                {/* Проверка правильности ответа */}
                {oIndex === question.correctIndex && (
                  <span className="text-green-600 ml-2">(Правильный ответ)</span>
                )}
              </li>
            ))}
          </ul>
        </div>
      ))}

      {/* Кнопка "Назад" */}
      <button
        onClick={() => navigate(-1)} // Переход на предыдущую страницу
        className="bg-gray-500 text-white p-2 rounded mb-4 hover:bg-gray-600"
      >
        Назад
      </button>
    </div>
  );
};

export default TestDetailPage;