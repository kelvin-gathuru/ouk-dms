export const storeToken = (data) => {

  try {
    localStorage.setItem('userToken', data.token);
    localStorage.setItem('name', data.name);
    console.log('Token stored successfully');
  } catch (error) {
    console.error('Error storing token:', error);
  }
};

export const getToken = () => {
  try {
    // Check new key first
    let token = localStorage.getItem('userToken');

    // Fallback to old key for backward compatibility
    if (!token) {
      token = localStorage.getItem('token');
      // If found in old location, migrate it to new location
      if (token) {
        localStorage.setItem('userToken', token);
        localStorage.removeItem('token');
      }
    }

    return token;
  } catch (error) {
    console.error('Error retrieving token:', error);
    return null;
  }
};

export const getUserName = () => {
  try {
    return localStorage.getItem('name');
  }
  catch (error) {
    console.error('Error retrieving name:', error);
    return null;
  }
}

export const removeToken = () => {
  try {
    localStorage.removeItem('userToken');
    localStorage.removeItem('name');
  } catch (error) {
    console.error('Error removing token:', error);
  }
};

export const logout = () => {
  removeToken();
  window.location.href = '/login';
};