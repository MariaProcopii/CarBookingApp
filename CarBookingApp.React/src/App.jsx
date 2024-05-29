import './App.css';
import { ThemeProvider } from '@emotion/react';
import { createTheme } from '@mui/material';
import { useEffect, useState } from 'react';
import AuthProvider from './components/provider/AuthProvider';
import Routes from './components/routes';

const themeOptionsLight = {
  palette: {
    type: 'light',
    primary: {
      main: '#a41116',
    },
    secondary: {
      main: '#3a8a69',
    },
    direction: 'rtl',
  },
};

const themeOptionsDark = {
  palette: {
    type: 'dark',
    primary: {
      main: '#560C0C',
    },
    secondary: {
      main: '#6811da',
    },
    background: {
      default: '#292929',
      paper: '#989292',
    },
    text: {
      primary: 'rgba(255,255,255,0.87)',
      secondary: 'rgba(255,255,255,0.54)',
      disabled: 'rgba(255,255,255,0.38)',
      hint: 'rgba(255,255,255,0.38)',
    },
  },
};

let theme = null;

function App() {
  const [isDarkThemeOn, setDarkTheme] = useState(() => {
    const savedState = localStorage.getItem("isDarkThemeOn");
    return savedState ? JSON.parse(savedState) : false;
  });

  if (!isDarkThemeOn) {
    theme = createTheme(themeOptionsLight);
  }
  else {
    theme = createTheme(themeOptionsDark);
  }

  useEffect(() => {
    localStorage.setItem("isDarkThemeOn", JSON.stringify(isDarkThemeOn));
  }, [isDarkThemeOn]);

  return (
    <ThemeProvider theme={theme}>
      <AuthProvider>
        <Routes />
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;
