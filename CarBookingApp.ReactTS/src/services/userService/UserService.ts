import { AxiosInstance } from "axios";
import { useNavigate } from "react-router-dom";
import { 
    LoginParams, 
    RegisterParams,
    DetailsParams,
    YearsOfExperienceParams,
} from "./types";
import { VehicleDetailsParams } from "../rideService/types";

function UserService(instance: AxiosInstance | null) {
    const navigate = useNavigate();

    async function login(
        loginParams: LoginParams,
        setToken: (newToken: string) => void,
    ): Promise<void> {
        try {
            const response = await instance?.post("/Auth/LogIn", loginParams);
            setToken(response?.data);
            navigate("/", { replace: true });
        } catch (error) {
            console.log(error);
        }
    }

    async function signup(
        registerParams: RegisterParams,
        setToken: (newToken: string) => void,
    ): Promise<void> {
        try {
            const response = await instance?.post("/Auth/SignUp", registerParams);
            setToken(response?.data);
            navigate("/", { replace: true });
        } catch (error) {
            console.log(error);
        }
    }

    async function fetchInfo(
        nameIdentifier: string,
        setUserInfo: (userDetails: DetailsParams) => void,
        setVehicleDetail: (vehicleDetails: VehicleDetailsParams) => void,
    ): Promise<void> {
        try {
            const response = await instance?.get(`/user/info/${nameIdentifier}`);
            setUserInfo(response?.data);
            setVehicleDetail(response?.data.vehicleDetail);
        } catch(error) {
            console.log(error);
        }
    }

    async function downgrade(
        nameIdentifier: string,
        setToken: (newToken: string) => void,
    ): Promise<void> {
        try {
            const response = await instance?.put(`/user/info/downgrade/${nameIdentifier}`);
            console.log(response?.data);
            setToken(response?.data);
        } catch (error) {
            console.log(error);
        }
    }

    async function updateDetails(
        nameIdentifier: string,
        userDetails: DetailsParams,
        setUserInfo: (userDetails: DetailsParams) => void,
        setToken: (newToken: string) => void,
        handleClose: () => void,
    ): Promise<void> {
        try {
            const response = await instance?.put(`/user/info/update/${nameIdentifier}`, userDetails);
            setUserInfo(userDetails);
            setToken(response?.data);
            handleClose();
        } catch(error) {
            console.log(error);
            console.log(nameIdentifier);
            console.log(userDetails);
        }
    }

    async function upgrade(
        nameIdentifier: string,
        userDetails: DetailsParams,
        yearsOfExperience: YearsOfExperienceParams,
        setToken: (newToken: string) => void,
        setUserInfo: (userDetails: DetailsParams) => void,
    ): Promise<void> {
        try {
            const response = await instance?.post(`/user/info/upgrade/${nameIdentifier}`, yearsOfExperience);
            setToken(response?.data);
            setUserInfo({
                ...userDetails,
                ...yearsOfExperience,
            });
        } catch(error) {
            console.log(error);
        }
    }

    async function approvePassengerForRide(
        infoParams: any,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const _ = await instance?.put("/user/pending/approve", infoParams);
            setSnackbar({ open: true, message: "Passener approved successfully!", severity: "success" });
        } catch(error) {
            console.log(error);
            setSnackbar({ open: true, message: "Failed to approve passenger from ride!", severity: "error" });

        }
    }

    async function rejectPassengerFromRide(
        infoParams: any,
        setSnackbar: (obj: {open: boolean; message: string; severity: string;}) => void,
    ): Promise<void> {
        try {
            const _ = await instance?.put("/user/pending/reject", infoParams);
            setSnackbar({ open: true, message: "Passenger rejected successfully!", severity: "success" });
        } catch(error) {
            console.log(error);
            setSnackbar({ open: true, message: "Failed to reject passenger from ride!", severity: "error" });
        } 
    }

    return {
        login, 
        signup,
        fetchInfo,
        downgrade,
        updateDetails,
        upgrade,
        approvePassengerForRide,
        rejectPassengerFromRide,
    };
}

export default UserService;