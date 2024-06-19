import useAuth from "../context/auth/UseAuth";
import { Navigate, Outlet } from "react-router-dom";

export default function ProtectedRoute() {
    const { token } = useAuth();

    if (!token) {
        return <Navigate to="/login" />;
    }

    return <Outlet />;
};