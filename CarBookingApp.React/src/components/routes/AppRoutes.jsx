import { ProtectedRoute } from "./ProtectedRoute";
import Login from "../../pages/login/Login";
import SignUp from "../../pages/signUp/SignUp";
import Logout from "../../pages/logout/Logout";
import AvailableRides from "../../pages/availableRides/AvailableRides";
import { Routes, Route } from 'react-router-dom';
import Sidebar from '../sidebar/Sidebar';
import Profile from "../../pages/profile/Profile";
import CreateRide from "../../pages/createRide/CreateRide";
import MyRides from "../../pages/myRides/MyRides";
import BookedRides from "../../pages/bookedRides/BookedRides";
import PendingRides from "../../pages/pendingRides/PendingRides";
import PendingPassengers from "../../pages/pendingPassengers/PendingPassengers";

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
            <Route path="profile" element={<Profile />} />
            <Route path="logout" element={<Logout />} />
            <Route path="create-ride" element={<CreateRide />} />
            <Route path="my-rides" element={<MyRides />} />
            <Route path="booked-rides" element={<BookedRides />} />
            <Route path="pending-rides" element={<PendingRides />}/>
            <Route path="pending-passengers" element={<PendingPassengers />}/>
          </Route>
      </Route>
    </Routes>
    </>
  );
};

export default AppRoutes;