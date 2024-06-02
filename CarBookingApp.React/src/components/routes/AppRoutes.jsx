import { ProtectedRoute } from "./ProtectedRoute";
import Login from "../../pages/login/Login";
import SignUp from "../../pages/signUp/SignUp";
import Logout from "../../pages/logout/Logout";
import AvailableRides from "../../pages/availableRides/AvailableRides";
import Example from "../example/Example";
import { Routes, Route } from 'react-router-dom';
import Sidebar from '../sidebar/Sidebar';


const AppRoutes = ({theme, isDarkThemeOn, setDarkTheme}) => {

  return (
    <>
    <Routes>
      <Route path="/service" element={<div>Service Page</div>} />
      <Route path="/about-us" element={<div>About Us</div>} />
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />
      <Route path="*" element={<div>Not Found</div>} />

      <Route path="/" element={<ProtectedRoute />}>
          <Route element={<Sidebar theme={theme} isDarkThemeOn={isDarkThemeOn} setDarkTheme={setDarkTheme}/>}>
            <Route index element={<AvailableRides />} />
            <Route path="profile" element={<div>User Profile</div>} />
            <Route path="logout" element={<Logout />} />
            <Route path="example/user" element={<Example />} />
          </Route>
      </Route>
    </Routes>
    </>
  );
};

export default AppRoutes;