import axios from 'axios';

const API_URL = `${process.env.REACT_APP_API_URL}/api/Block`;

export const getAllBlocks = async () => {
  try {
    const response = await axios.get(API_URL);
    return response.data;
  } catch (error) {
    console.error('Error fetching blocks', error);
    throw error;
  }
};

export const getBlockById = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching block by ID', error);
    throw error;
  }
};

export const createBlock = async (blockData) => {
  try {
    const response = await axios.post(API_URL, blockData);
    return response.data;
  } catch (error) {
    console.error('Error creating block', error);
    throw error;
  }
};

export const updateBlock = async (id, blockData) => {
  try {
    const response = await axios.put(`${API_URL}/${id}`, blockData);
    return response.data;
  } catch (error) {
    console.error('Error updating block', error);
    throw error;
  }
};

export const deleteBlock = async (id) => {
  try {
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error deleting block', error);
    throw error;
  }
};

export const getBlockWithTopics = async (id) => {
  try {
    const response = await axios.get(`${API_URL}/with-topics/${id}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching block with topics', error);
    throw error;
  }
};