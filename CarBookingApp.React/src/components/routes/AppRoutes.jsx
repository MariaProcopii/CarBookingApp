import { ProtectedRoute } from "./ProtectedRoute";
import Login from "../../pages/login/Login";
import SignUp from "../../pages/signUp/SignUp";
import Logout from "../../pages/logout/Logout";
import Example from "../example/Example";
import { Routes, Route } from 'react-router-dom';

const AppRoutes = () => {

  return (
    <Routes>
      <Route path="/service" element={<div>Service Page</div>} />
      <Route path="/about-us" element={<div>About Us</div>} />
      <Route path="/" element={<div>Home Page</div>} />
      <Route path="/login" element={<Login />} />
      <Route path="/signup" element={<SignUp />} />
      <Route path="*" element={<div>Not Found</div>} />

      <Route path="/" element={<ProtectedRoute />}>
          <Route index element={<div>User Home Page</div>} />
          <Route path="profile" element={<div>User Profile</div>} />
          <Route path="logout" element={<Logout />} />
          <Route path="example/user" element={<Example />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;