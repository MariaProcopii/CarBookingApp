import { useState } from 'react';
import { useTheme } from '@mui/material/styles';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import CustomSnackbar from '../../components/customSnackbar/CustomSnackbar';
import axios from 'axios';
import dayjs from 'dayjs';
import RideForm from '../../components/rideForm/RideForm';

export default function CreateRide() {

    const rideDefaultData = {
        dateOfTheRide: dayjs(),
        DestinationFrom: "",
        DestinationTo: "",
        TotalSeats: 1,
        RideDetail: {
            PickUpSpot: "",
            Price: "",
            Facilities: [],
        },
    };
    
    const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });
    const [rideData, setRideData] = useState({...rideDefaultData});
    const { token } = useAuth();
    const claims = useTokenDecoder(token);
    const theme = useTheme();

    const createRide = (rideData) => {
        axios.post(`http://192.168.0.9:5239/ride/create/${claims.nameidentifier}`, rideData)
            .then((response) => {
                setSnackbar({ open: true, message: 'Ride created successfully!', severity: 'success' });
            })
            .catch((error) => {
                setSnackbar({ open: true, message: 'Failed to create ride.', severity: 'error' });
                console.log('Error creating ride:', error.data);
            });
    };
    

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    return (
        <>
            <RideForm 
                rideData={rideData}
                handleSubmit={createRide}
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