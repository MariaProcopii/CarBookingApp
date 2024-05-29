import { useAuth } from "../../components/provider/AuthProvider";

const Logout = () => {
  const { setToken } = useAuth();
  setToken(null);
};

export default Logout;