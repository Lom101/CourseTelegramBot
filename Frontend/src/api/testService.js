import axios from 'axios';

const API_URL = `${process.env.REACT_APP_API_URL}/api/FinalTest`;

// Функция для получения всех тестов
export const getAllTests = async () => {
  try {
    const response = await axios.get(`${API_URL}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching tests:', error);
    throw error;
  }
};

// Функция для создания нового теста
export const createTest = async (testData) => {
  try {
    const response = await axios.post(API_URL, testData);
    return response.data;
  } catch (error) {
    console.error('Error creating test:', error);
    throw error;
  }
};

// Функция для получения теста по айди
export const getTestById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching test by ID:', error);
    throw error;
  }
};