import axios from 'axios';

const API_URL = `${process.env.REACT_APP_API_URL}/api/ContentItem`;

export const getContentItemById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching content item by ID', error);
    throw error;
  }
};

export const getContentItemsByTopic = async (topicId) => {
  try {
    const response = await axios.get(`${API_URL}/by-topic/${topicId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching content items by topic', error);
    throw error;
  }
};

export const deleteContentItem = async (id) => {
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting content item', error);
    throw error;
  }
};

export const createWordContentItem = async (formData) => {
  const response = await axios.post(`${API_URL}/word`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return response.data;
};

export const createImageContentItem = async (formData) => {
  const response = await axios.post(`${API_URL}/image`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return response.data;
};

export const createBookContentItem = async (formData) => {
  const response = await axios.post(`${API_URL}/book`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return response.data;
};

export const createAudioContentItem = async (formData) => {
  const response = await axios.post(`${API_URL}/audio`, formData, {
    headers: { 'Content-Type': 'multipart/form-data' },
  });
  return response.data;
};
