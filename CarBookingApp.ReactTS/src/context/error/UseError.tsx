import ErrorContext from "./ErrorContext";
import { useContext } from "react";

export default function useError() {
    return useContext(ErrorContext);
}
