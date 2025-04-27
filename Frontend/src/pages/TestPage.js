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
    <div className="min-h-screen bg-gray-100 px-10 py-8">
      <div className="mb-8 text-center">
        <h2 className="text-4xl font-extrabold text-gray-800 mb-2">Список тестов</h2>
        <div className="h-1 w-24 bg-gray-300 rounded-full mx-auto" />
      </div>
      <button 
        className="px-6 py-2 bg-gray-700 hover:bg-gray-800 text-white font-semibold rounded-xl transition h-fit" 
        onClick={handleAddQuestion}>
        Добавить вопрос
      </button>

      {/* Раздел добавленных вопросов */}
      {newTestQuestions.map((question, qIndex) => (
        <div key={qIndex} className="mb-6 w-full">
          <h3 className="text-xl font-extrabold text-gray-800 mt-2">
            Вопрос {qIndex + 1}:{' '}
            <span onClick={() => handleEditQuestion(qIndex)} className="text-xl font-extrabold text-gray-800 mb-2 underline">
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