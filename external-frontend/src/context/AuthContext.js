import { createContext,useContext,useState,useEffect } from "react";
import { storeToken,getToken,removeToken} from '../service/tokenStorage';

export const AuthContext = createContext();
export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [isAuthenticated, setIsAuthenticated] = useState(!!getToken());

  //Restore user data on initial load

//   useEffect(() => {
//     const token = getToken();
//     if (token) {
//      fetch('http://',{
//         method: 'GET',
//         headers: {
//           'Authorization': `Bearer ${token}`,
//         },
//       })
//       .then(response => response.json())
//       .then(data => {
//         if (data.success) {
//           setUser(data.user);
//           setIsAuthenticated(true);
//         } else {
//           removeToken();
//           setIsAuthenticated(false);
//         }
//       })
//       .catch(error => {
//         console.error('Error fetching user data:', error);
//         removeToken();
//         setIsAuthenticated(false);
//      })
       
//     }
//   }, []);

  const login = (data) => {
    setUser(data);
    setIsAuthenticated(true);
    storeToken(data);
  };

  const logout = () => {
    setUser(null);
    setIsAuthenticated(false);
    removeToken();
  };

  return (
    <AuthContext.Provider value={{ user, isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}