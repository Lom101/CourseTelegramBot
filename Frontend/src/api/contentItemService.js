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

export const createWordContentItem = async (data) => {
  try {
    const response = await axios.post(`${API_URL}/word`, data);
    return response.data;
  } catch (error) {
    console.error('Error creating word content item', error);
    throw error;
  }
};

export const createImageContentItem = async (data) => {
  try {
    const response = await axios.post(`${API_URL}/image`, data);
    return response.data;
  } catch (error) {
    console.error('Error creating image content item', error);
    throw error;
  }
};

export const createBookContentItem = async (data) => {
  try {
    const response = await axios.post(`${API_URL}/book`, data);
    return response.data;
  } catch (error) {
    console.error('Error creating book content item', error);
    throw error;
  }
};

export const createAudioContentItem = async (data) => {
  try {
    const response = await axios.post(`${API_URL}/audio`, data);
    return response.data;
  } catch (error) {
    console.error('Error creating audio content item', error);
    throw error;
  }
};