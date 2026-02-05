import{jwtDecode} from 'jwt-decode';
import { getToken } from './tokenStorage';

export function getUserName() {
  const token = getToken('token');
  if (!token) {
    return null; // No token found
  }

  try {
    const decodedToken = jwtDecode(token);
    return decodedToken.name ; 
  } catch (error) {
    console.error('Error decoding token:', error);
    return null; 
  }
}