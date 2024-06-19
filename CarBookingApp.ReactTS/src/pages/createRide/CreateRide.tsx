import dayjs from "dayjs";
import RideForm from "../../components/rideForm/RideForm";
import CustomSnackbar from "../../components/customSnackbar/CustomSnackbar";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import { useState } from "react";

export default function CreateRide() {
    const rideDefaultData = {
        dateOfTheRide: dayjs(),
        destinationFrom: "",
        destinationTo: "",
        totalSeats: 1,
        rideDetail: {
            pickUpSpot: "",
            price: "",
            facilities: [],
        },
    };
    
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });
    const [rideData, _] = useState({...rideDefaultData});

    const { instance } = useAPI();
    const { createRide } = RideService(instance);

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    return (
        <>
            <RideForm 
                rideData={rideData}
                handleSubmit={createRide}
                setSnackbar={setSnackbar}
                titleText={"Create Ride"}
            />
            <CustomSnackbar 
                open={snackbar.open} 
                message={snackbar.message} 
                severity={snackbar.severity} 
                onClose={handleCloseSnackbar} 
            />
        </>
    );
}