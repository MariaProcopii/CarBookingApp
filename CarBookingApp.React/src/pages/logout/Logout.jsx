import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/provider/AuthProvider";

const Logout = () => {
  const { setToken } = useAuth();

  const navigate = useNavigate();

  const handleLogout = () => {
    setToken(null);
    navigate("/", { replace: true });
  };

  setTimeout(() => {
    handleLogout();
  }, 5 * 1000);

  return <>Logout Page</>;
};

export default Logout;