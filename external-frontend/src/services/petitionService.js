import axios from 'axios';
import config from '../config';

export const fetchPetitions = async () => {
  try {
    const response = await axios.get(`${config.api.baseURL}/Documents/public/petitions`);
    return response.data;
  } catch (error) {
    console.error('Error fetching petitions:', error);
    throw error;
  }
};

export const getPetitionStatus = (status) => {
  const statusMap = {
    'SUBMITTED': 'Submitted',
    'UNDER_REVIEW': 'Under Review',
    'RESOLVED': 'Resolved',
    'REJECTED': 'Rejected'
  };
  return statusMap[status] || status;
};

// Calculate due date (14 days from created date)
export const calculateDueDate = (createdDate) => {
  const date = new Date(createdDate);
  date.setDate(date.getDate() + 14);
  return date.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric'
  });
};

// Format date to a more readable format
export const formatDate = (dateString) => {
  if (!dateString) return '';
  const options = { year: 'numeric', month: 'short', day: 'numeric' };
  return new Date(dateString).toLocaleDateString('en-US', options);
};
