import ThemeContext from "./ThemeContext";
import { createTheme, PaletteMode } from "@mui/material";
import { ThemeProvider, Theme } from "@mui/material/styles";
import { useEffect, useState, useMemo } from "react";

interface Props {
    children: React.ReactNode;
}

export default function CustomThemeProvider({ children }: Props) {
    const themeOptionsLight = {
        palette: {
            mode: "light" as PaletteMode,
            primary: {
                main: "#a41116",
            },
            secondary: {
                main: "#3a8a69",
            },
            direction: "rtl",
        },
    };
    const themeOptionsDark = {
        palette: {
            mode: "dark" as PaletteMode,
            primary: {
                main: "#993333",
            },
            secondary: {
                main: "#993333",
            },
            background: {
                default: "#1E1E1D",
                paper: "#1E1E1E",
            },
            text: {
                primary: "rgba(255,255,255,0.87)",
                secondary: "rgba(255,255,255,0.54)",
                disabled: "rgba(255,255,255,0.38)",
                hint: "rgba(255,255,255,0.38)",
            },
        },
    };

    const [theme, setTheme] = useState<Theme | null>(null);
    const [isDarkThemeOn, setDarkTheme] = useState(() => {
        const savedState = localStorage.getItem("isDarkThemeOn");
        return savedState ? JSON.parse(savedState) : false;
    });

    useEffect(() => {
        localStorage.setItem("isDarkThemeOn", JSON.stringify(isDarkThemeOn));
        if (!isDarkThemeOn) {
            setTheme(createTheme(themeOptionsLight));
        }
        else {
            setTheme(createTheme(themeOptionsDark));
        }
    }, [isDarkThemeOn]);

    const contextValue = useMemo(() => ({
        theme,
        isDarkThemeOn,
        setDarkTheme,
    }), [isDarkThemeOn]);

    return (
        <ThemeContext.Provider value={contextValue}>
            <ThemeProvider
                theme={
                    theme
                        ?
                        theme
                        :
                        createTheme(
                            !isDarkThemeOn
                                ?
                                createTheme(themeOptionsLight)
                                :
                                createTheme(themeOptionsDark)
                        )
                }
            >
                {children}
            </ThemeProvider>
        </ThemeContext.Provider>
    );
}

