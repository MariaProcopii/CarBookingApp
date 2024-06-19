import useAuth from "../../context/auth/UseAuth";

export default function Logout() {
    const { setToken } = useAuth();
    setToken("");

    return <></>;
}