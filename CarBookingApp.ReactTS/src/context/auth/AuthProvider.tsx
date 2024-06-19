import React, { useEffect } from "react";
import AuthContext from "./AuthContext";
import { useMemo, useState } from "react";

interface Props {
    children: React.ReactNode;
}

export default function AuthProvider({ children }: Props) {
    const [token, setToken] = useState(localStorage.getItem("token"));

    useEffect(() => {
        localStorage.setItem("token", token ? token : "");
    }, [token]);

    const contextValue = useMemo(() => ({
        token,
        setToken,
    }), [token]);

    return (
        <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>
    );
}