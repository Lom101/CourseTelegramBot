import React, { useState, useEffect } from "react";
import { getAllUserProgress } from "../api/userProgressService";

const UsersProgressPage = () => {
  const [usersProgress, setUsersProgress] = useState([]);
  const [loading, setLoading] = useState(true); // Флаг загрузки

  useEffect(() => {
    const fetchUsersProgress = async () => {
      try {
        const data = await getAllUserProgress();
        setUsersProgress(data);  // Сохраняем всех пользователей с их прогрессом
      } catch (error) {
        console.error("Ошибка при загрузке прогресса пользователей:", error);
      } finally {
        setLoading(false); // Останавливаем индикатор загрузки
      }
    };

    fetchUsersProgress();
  }, []);

  if (loading) {
    return <div className="text-center text-xl font-semibold py-20">Загрузка...</div>;
  }

  return (
    <div className="min-h-screen bg-gray-50 px-8 py-12">
      <h2 className="text-4xl font-extrabold text-gray-900 text-center mb-10">Прогресс пользователей</h2>
      <div className="h-1 w-24 bg-gradient-to-r from-blue-500 to-green-500 rounded-full mx-auto mb-10" />

      {usersProgress.length === 0 ? (
        <p className="text-center text-lg text-gray-500">Нет данных о прогрессе пользователей.</p>
      ) : (
        usersProgress.map((user) => (
          <div key={user.userId} className="bg-white p-6 rounded-lg shadow-lg mb-8">
            <div className="flex items-center mb-4">
              <div className="text-xl font-bold text-gray-800 mr-4">{user.fullName}</div>
              <span className="text-sm text-gray-500">{user.email}</span>
            </div>
            <div className="space-y-6">
              {user.blockProgresses.length === 0 ? (
                <p className="text-center text-sm text-gray-400">Прогресс по блокам не найден.</p>
              ) : (
                user.blockProgresses.map((block, blockIdx) => (
                  <div key={blockIdx} className="bg-gray-100 p-4 rounded-lg shadow-sm">
                    <h4 className="text-lg font-semibold text-gray-700 mb-2">{block.blockTitle}</h4>
                    <p className="text-sm text-gray-600">Статус блока: {block.isBlockCompleted ? 'Завершено' : 'Не завершено'}</p>

                    <div className="mt-4 space-y-2">
                      {block.topicProgresses.map((topic, topicIdx) => (
                        <div key={topicIdx} className="flex items-center justify-between">
                          <span className="text-sm text-gray-500">Тема {topic.topicId}</span>
                          <span className={`text-sm font-semibold ${topic.isTopicCompleted ? 'text-green-500' : 'text-red-500'}`}>
                            {topic.isTopicCompleted ? 'Завершена' : 'Не завершена'}
                          </span>
                        </div>
                      ))}
                    </div>

                    {block.finalTestProgress && (
                      <div className="mt-4 p-4 bg-blue-50 rounded-lg">
                        <p className="text-sm text-gray-700">Финальный тест: <span className={block.finalTestProgress.isPassed ? 'text-green-500' : 'text-red-500'}>{block.finalTestProgress.isPassed ? 'Пройден' : 'Не пройден'}</span></p>
                        <p className="text-sm text-gray-600">Ответов правильно: {block.finalTestProgress.correctAnswersCount}</p>
                        <p className="text-sm text-gray-500">Пройден: {new Date(block.finalTestProgress.passedAt).toLocaleString()}</p>
                      </div>
                    )}
                  </div>
                ))
              )}
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default UsersProgressPage;
