import Login from "../pages/login/Login";
import SignUp from "../pages/signUp/SignUp";
import Logout from "../pages/logout/Logout";
import AvailableRides from "../pages/availableRides/AvailableRides";
import Sidebar from "../components/sidebar/Sidebar";
import Profile from "../pages/profile/Profile";
import ProtectedRoute from "./ProtectedRoute";
import CreateRide from "../pages/createRide/CreateRide";
import MyRides from "../pages/myRides/MyRides";
import BookedRides from "../pages/bookedRides/BookedRides";
import PendingRides from "../pages/pendingRides/PendingRides";
import PendingPassengers from "../pages/pendingPassengers/PendingPassengers";
import { Routes, Route } from "react-router-dom";
import { ExecutePayment } from "../components/executePaymen/ExecutePayment";

export default function AppRoutes() {

    return (
        <>
            <Routes>
                <Route path="/login" element={<Login />} />
                <Route path="/signup" element={<SignUp />} />
                <Route path="*" element={<div>Not Found</div>} />
                <Route path="/internal-error" element={<div>Internal Server Error</div>} />

                <Route path="/" element={<ProtectedRoute />}>
                    <Route element={<Sidebar />}>
                        <Route index element={<AvailableRides />} />
                        <Route path="profile" element={<Profile />} />
                        <Route path="logout" element={<Logout />} />
                        <Route path="create-ride" element={<CreateRide />} />
                        <Route path="my-rides" element={<MyRides />} />
                        <Route path="booked-rides" element={<BookedRides />} />
                        <Route path="pending-rides" element={<PendingRides />} />
                        <Route path="pending-passengers" element={<PendingPassengers />}/>
                        <Route path="/payment/execute" element={<ExecutePayment />} />
                    </Route>
                </Route>
            </Routes>
        </>
    );
};