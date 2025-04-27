import React from 'react';

const QuestionModal = ({
  isModalOpen, setIsModalOpen, currentQuestionIndex, 
  newTestQuestions, setNewTestQuestions, currentOption, setCurrentOption, handleCloseModal, isEditing
}) => {
  const handleQuestionChange = (value) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[currentQuestionIndex].questionText = value;
    setNewTestQuestions(updatedQuestions);
  };

  const handleOptionChange = (index, value) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[currentQuestionIndex].options[index].optionText = value;
    setNewTestQuestions(updatedQuestions);
  };

  const handleAddOption = () => {
    if (!currentOption.trim()) return alert('Вариант ответа не может быть пустым!');
    
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[currentQuestionIndex].options.push({ optionText: currentOption });
    setNewTestQuestions(updatedQuestions);
    setCurrentOption('');  // Очистить поле для ввода варианта
  };

  const handleRemoveOption = (oIndex) => {
    const updatedQuestions = [...newTestQuestions];
    updatedQuestions[currentQuestionIndex].options.splice(oIndex, 1); // Удаляем вариант
    setNewTestQuestions(updatedQuestions);
  };

  const handleSaveQuestion = () => {
    const question = newTestQuestions[currentQuestionIndex];
    if (!question.questionText.trim()) return alert('Вопрос не может быть пустым!');
    if (question.options.length < 2 || question.options.some(option => !option.optionText.trim())) {
      return alert('Для каждого вопроса должно быть как минимум 2 варианта ответа!');
    }

    setIsModalOpen(false);
  };

  return (
    isModalOpen && (
      <div className="fixed inset-0 bg-gray-600 bg-opacity-50 flex justify-center items-center z-50">
        <div className="bg-white p-6 rounded-lg shadow-md w-1/3">
          <h2 className="text-2xl mb-4 text-gray-800">
            {isEditing ? 'Редактирование вопроса' : 'Добавление вопроса'}
          </h2>
          <input
            type="text"
            value={newTestQuestions[currentQuestionIndex]?.questionText || ''}
            onChange={(e) => handleQuestionChange(e.target.value)}
            placeholder="Введите текст вопроса"
            className="border p-3 w-full mb-4 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          />
          <div className="space-y-4">
            {newTestQuestions[currentQuestionIndex]?.options.map((option, oIndex) => (
              <div key={oIndex} className="flex justify-between items-center">
                <input
                  type="text"
                  value={option.optionText}
                  onChange={(e) => handleOptionChange(oIndex, e.target.value)}
                  placeholder={`Вариант ${oIndex + 1}`}
                  className="border p-3 w-full mb-4 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
                />
                <button
                  className="bg-red-500 text-white p-2 rounded ml-2"
                  onClick={() => handleRemoveOption(oIndex)} // Обработчик удаления
                >
                  Удалить
                </button>
              </div>
            ))}
            <input
              type="text"
              value={currentOption}
              onChange={(e) => setCurrentOption(e.target.value)}
              placeholder="Введите новый вариант ответа"
              className="border p-3 w-full mb-4 rounded-lg shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
            <button
              className="bg-blue-500 text-white p-2 rounded w-48 h-12 hover:bg-blue-600"
              onClick={handleAddOption}
            >
              Добавить вариант
            </button>
          </div>
          <button className="bg-green-500 text-white p-2 rounded w-48 h-12 hover:bg-green-600 mt-3" onClick={handleSaveQuestion}>
            Сохранить
          </button>

          <button 
            className="bg-gray-500 text-white p-2 rounded w-48 h-12 hover:bg-gray-600 mt-3 ml-2" 
            onClick={handleCloseModal}
          >
            Отмена
          </button>
        </div>
      </div>
    )
  );
};


export default QuestionModal;
