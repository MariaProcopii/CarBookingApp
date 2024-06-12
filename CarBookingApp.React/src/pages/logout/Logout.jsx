import { useAuth } from "../../components/provider/AuthProvider";

const Logout = () => {
  const { setToken } = useAuth();
  setTimeout(() => {
    setToken(null);
}, 10);
};

export default Logout;