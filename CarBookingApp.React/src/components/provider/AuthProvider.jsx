import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { createContext, useContext, useEffect, useMemo, useState } from "react";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  // State to hold the authentication token
  const [token, setToken_] = useState(localStorage.getItem("token"));

  // Function to set the authentication token
  const setToken = (newToken) => {
    setToken_(newToken);
  };

  // Function that check TTL of JWT
  const isTokenExpired = () => {
    console.log("enter func");
    if (!token) return;
    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      console.log(currentTime);
      console.log(decodedToken.exp);
      if (decodedToken.exp < currentTime) {
        delete axios.defaults.headers.common["Authorization"];
        localStorage.removeItem("token");
      }
    } catch (error) {
      console.error('Error decoding token:', error);
      return true;
    }
  };

  useEffect(() => {
    if (token) {
      axios.defaults.headers.common["Authorization"] = "Bearer " + token;
      localStorage.setItem("token", token);
    } else {
      delete axios.defaults.headers.common["Authorization"];
      localStorage.removeItem("token");
    }
  }, [token]);

  useEffect(() => {
    if (!token) return;
    const intervalId = setInterval(isTokenExpired, 120000);
    console.log("Timer start");
    setTimeout(()=>{console.log("done counting")}, 120000);
    return () => clearInterval(intervalId);
  }, []);

  // Memoized value of the authentication context
  const contextValue = useMemo(
    () => ({
      token,
      setToken,
    }),
    [token]
  );

  // Provide the authentication context to the children components
  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;