import { createContext } from "react";
import { ErrorObject } from "../../utils/ErrorUtils";

interface ErrorContext {
    error: React.MutableRefObject<ErrorObject>;
    setError: (newError: string) => void;
}

const ErrorContext = createContext<ErrorContext>({} as ErrorContext);

export default ErrorContext; 