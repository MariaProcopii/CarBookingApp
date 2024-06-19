import React from "react"; 
import ErrorContext from "./ErrorContext";
import { useRef } from "react";
import { useMemo, useState } from "react";
import { ErrorObject, parseErrorMessages } from "../../utils/ErrorUtils";

interface Props {
    children: React.ReactNode;
}

export default function ErrorProvider({ children }: Props) {
    const [_error, _setError] = useState<ErrorObject>({} as ErrorObject);
    const error = useRef({} as ErrorObject);
    error.current = _error;

    const setError = (newError: string) => {
        _setError(parseErrorMessages(newError));
    };

    const contextValue = useMemo(() => ({
        error,
        setError,
    }), [_error]);

    return (
        <ErrorContext.Provider value={contextValue}>{children}</ErrorContext.Provider>
    );
}