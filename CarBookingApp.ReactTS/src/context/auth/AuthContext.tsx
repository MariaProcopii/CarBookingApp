import { createContext } from "react";

interface AuthContext {
    token: string | null;
    setToken: (newToken: string) => void;
}

const AuthContext = createContext<AuthContext>({} as AuthContext);

export default AuthContext;