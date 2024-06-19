import axios from "axios";
import React from "react";
import useAuth from "../auth/UseAuth";
import useError from "../error/UseError";
import APIContext from "./APIContext";
import { useMemo } from "react";
import { ErrorObject } from "../../utils/ErrorUtils";
import { useNavigate } from "react-router-dom";
import {
    AxiosResponse, AxiosError, InternalAxiosRequestConfig
} from "axios";

interface Props {
    children: React.ReactNode;
}

export default function APIProvider({ children }: Props) {
    const navigate = useNavigate();
    const { token } = useAuth();
    const { setError } = useError();

    const instance = axios.create({
        baseURL: "http://192.168.0.9:5239/",
        headers: {
            "Authorization": "",
        },
    });

    instance.interceptors.request.use(
        (config: InternalAxiosRequestConfig) => {
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error: AxiosError) => {
            return Promise.reject(error);
        },
    );

    instance.interceptors.response.use(
        (response: AxiosResponse) => {
            return response;
        },
        (error: AxiosError<ErrorObject>) => {
            const status = error.response ? error.response.status : null;
            const message = (
                error.response && error.response.data
            ) ? error.response.data.Message : null;

            if (status === 401) {
                navigate("/login", { replace: true });
            } else if (status === 404) {
                navigate("*", { replace: true });
            } else if (message) {
                setError(message);
            } else {
                // navigate("/internal-error", { replace: true });
            }

            return Promise.reject(error);
        },
    );

    const contextValue = useMemo(() => ({
        instance,
    }), [instance]);

    return (
        <APIContext.Provider value={contextValue}>{children}</APIContext.Provider>
    );
}