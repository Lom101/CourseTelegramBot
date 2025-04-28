import axios from 'axios';

const API_URL = `${process.env.REACT_APP_API_URL}/api/UserProgress`;

export const getAllUserProgress = async () => {
  try {
    const response = await axios.get(`${API_URL}`);  // Новый эндпоинт для получения всех пользователей с прогрессом
    return response.data;  // Возвращаем список пользователей с прогрессом
  } catch (error) {
    console.error('Ошибка при загрузке прогресса пользователей:', error);
    throw error;
  }
};
