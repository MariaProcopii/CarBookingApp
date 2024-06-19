import ThemeContext from "./ThemeContext";
import { useContext } from "react";

export default function useTheme() {
    return useContext(ThemeContext);
}