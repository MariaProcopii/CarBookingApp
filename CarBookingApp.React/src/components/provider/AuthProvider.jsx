import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { createContext, useContext, useEffect, useMemo, useState } from "react";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [token, setToken_] = useState(localStorage.getItem("token"));


  const setToken = (newToken) => {
    setToken_(newToken);
  };


  const isTokenExpired = () => {
    console.log("enter func");
    if (!token) return;
    try {
      const decodedToken = jwtDecode(token);
      const currentTime = Date.now() / 1000;
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
  
    const intervalDuration = 120000;
    const storedStartTime = localStorage.getItem('tokenTimerStart');
  
    let startTime = storedStartTime ? parseInt(storedStartTime, 10) : Date.now();
  
    if (!storedStartTime) {
      localStorage.setItem('tokenTimerStart', startTime);
    }
  
    const calculateRemainingTime = () => {
      const currentTime = Date.now();
      const elapsedTime = currentTime - startTime;
      return intervalDuration - (elapsedTime % intervalDuration);
    };
  
    const setTimer = (remainingTime) => {
      const timeoutId = setTimeout(() => {
        isTokenExpired();
        startTime = Date.now();
        localStorage.setItem('tokenTimerStart', startTime);
        setTimer(intervalDuration);
      }, remainingTime);
  
      return () => clearTimeout(timeoutId);
    };
  
    const remainingTime = calculateRemainingTime();
    const clearTimer = setTimer(remainingTime);
  
    console.log('Timer start');
  
    return () => {
      clearTimer();
      localStorage.removeItem('tokenTimerStart');
    };
  }, [token]);


  const contextValue = useMemo(
    () => ({
      token,
      setToken,
    }),
    [token]
  );


  return (
    <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
  );
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export default AuthProvider;