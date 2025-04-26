import axios from 'axios';

const API_URL = `${process.env.REACT_APP_API_URL}/api/Topic`;

export const getAllTopicsByBlockId = async (blockId) => {
  try {
    const response = await axios.get(`${API_URL}/all-by-block/${blockId}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching topics by block ID', error);
    throw error;
  }
};

export const getTopicById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching topic by ID', error);
    throw error;
  }
};

export const createTopic = async (topicData) => {
  try {
    const response = await axios.post(API_URL, topicData);
    return response.data;
  } catch (error) {
    console.error('Error creating topic', error);
    throw error;
  }
};

export const updateTopic = async (id, topicData) => {
  try {
    const response = await axios.put(`${API_URL}/${id}`, topicData);
    return response.data;
  } catch (error) {
    console.error('Error updating topic', error);
    throw error;
  }
};

export const deleteTopic = async (id) => {
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting topic', error);
    throw error;
  }
};
