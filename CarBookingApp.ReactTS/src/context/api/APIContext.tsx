import { AxiosInstance } from "axios";
import { createContext } from "react";

interface APIContext {
    instance: AxiosInstance | null;
}

const APIContext = createContext<APIContext>({} as APIContext);

export default APIContext;