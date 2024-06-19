import { Theme } from "@mui/material";
import { createContext } from "react";

interface ThemeContext {
    theme: Theme | null;
    isDarkThemeOn: boolean | null;
    setDarkTheme: (isDarkThemeOn: boolean) => void;
}

const ThemeContext = createContext<ThemeContext>({} as ThemeContext);

export default ThemeContext;