import axios from "axios";
import { useAuth } from "../../components/provider/AuthProvider";

const Logout = () => {
  const { token } = useAuth();

  const handleRequest = () => {
    console.log(token);
    axios.get("http://localhost:5239/ride/21")
    .then((response) => {console.log(response.data)})
    .catch((error) => {console.log(error)});
  };

  setTimeout(() => {
    handleRequest();
  }, 5 * 1000);

  return <>Example Page</>;
};

export default Logout;