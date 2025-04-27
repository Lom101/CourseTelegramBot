import React, { useEffect, useState } from 'react'; 
import { getAllTests, createTest } from '../api/testService';
import QuestionModal from '../components/QuestionModal';
import TestList from '../components/TestList';
import TestForm from '../components/TestForm';

const TestPage = () => {
  const [tests, setTests] = useState([]);
  const [newTestTitle, setNewTestTitle] = useState('');
  const [newTestQuestions, setNewTestQuestions] = useState([]);
  const [currentQuestionIndex, setCurrentQuestionIndex] = useState(null);
  const [currentOption, setCurrentOption] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isTestBeingCreated, setIsTestBeingCreated] = useState(false);

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

  const handleCreateTest = async () => {
    if (newTestTitle.trim() === '') return alert('Название теста не может быть пустым!');
    
    const isDuplicateTitle = tests.some(test => test.title === newTestTitle);
    if (isDuplicateTitle) return alert('Тест с таким названием уже существует!');

    for (let question of newTestQuestions) {
      if (!question.questionText.trim()) return alert('Все вопросы должны быть заполнены!');
      if (question.options.length < 2 || question.options.some(option => !option.optionText.trim())) {
        return alert('Каждый вопрос должен иметь минимум 2 варианта ответа!');
      }
    }

    try {
      await createTest({ title: newTestTitle, questions: newTestQuestions });
      setNewTestTitle('');
      setNewTestQuestions([]);
      setIsModalOpen(false);
      const data = await getAllTests();
      setTests(data);
      setIsTestBeingCreated(false);
    } catch (error) {
      console.error('Ошибка при создании теста:', error);
    }
  };

  const handleAddQuestion = () => {
    setCurrentQuestionIndex(newTestQuestions.length); // Устанавливаем индекс последнего вопроса
    setNewTestQuestions([...newTestQuestions, { questionText: '', options: [] }]);
    setIsModalOpen(true); // Открытие модального окна
    setIsTestBeingCreated(true); // Это создание нового вопроса
  };

  const handleCloseModal = () => {
    // Если был добавлен новый вопрос, удалим его из массива
    if (isTestBeingCreated) {
      setNewTestQuestions(prevQuestions => prevQuestions.slice(0, -1)); // Удаляем последний вопрос
    }
    
    setIsModalOpen(false); // Закрываем модальное окно
    setCurrentQuestionIndex(null); // Сбрасываем индекс текущего вопроса
    setCurrentOption(''); // Сбрасываем текущий вариант ответа
  };

  const handleEditQuestion = (index) => {
    setCurrentQuestionIndex(index);  // Устанавливаем индекс редактируемого вопроса
    setIsModalOpen(true); // Открываем модальное окно для редактирования вопроса
    setIsTestBeingCreated(false); // Указываем, что это редактирование, а не добавление
  };

  return (
    <div className="m-10">
      <h1 className="text-2xl mb-4">Список тестов</h1>
      <button 
        className="bg-green-500 text-white p-2 rounded w-48 h-12 hover:bg-green-600 mt-2" 
        onClick={handleAddQuestion}>
        Добавить вопрос
      </button>

      {/* Раздел добавленных вопросов */}
      {newTestQuestions.map((question, qIndex) => (
        <div key={qIndex} className="mb-6 w-full">
          <h3 className="text-xl text-gray-700">
            Вопрос {qIndex + 1}:{' '}
            <span onClick={() => handleEditQuestion(qIndex)} className="text-blue-500 cursor-pointer">
              {question.questionText}
            </span>
          </h3>
          <ul>
            {question.options.map((option, oIndex) => (
              <li key={oIndex} className="mb-2">{`${oIndex + 1}. ${option.optionText}`}</li>
            ))}
          </ul>
        </div>
      ))}

      {isTestBeingCreated && (
        <TestForm 
          newTestTitle={newTestTitle} 
          setNewTestTitle={setNewTestTitle} 
          handleCreateTest={handleCreateTest} 
        />
      )}

      <QuestionModal
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        currentQuestionIndex={currentQuestionIndex}
        newTestQuestions={newTestQuestions}
        setNewTestQuestions={setNewTestQuestions}
        currentOption={currentOption}
        setCurrentOption={setCurrentOption}
        handleCloseModal={handleCloseModal}
        isEditing={currentQuestionIndex !== null}
      />

      <TestList tests={tests} />
    </div>
  );
};

export default TestPage;