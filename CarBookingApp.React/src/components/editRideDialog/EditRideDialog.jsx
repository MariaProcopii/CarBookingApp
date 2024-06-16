import { Dialog, DialogTitle, DialogContent, Typography, IconButton } from "@mui/material";
import CloseIcon from '@mui/icons-material/Close';
import RideForm from "../rideForm/RideForm";
import CustomSnackbar from "../customSnackbar/CustomSnackbar";
import { useState } from "react";
import axios from "axios";

export default function EditRideDialog({ open, setOpen, rideDetails, setRideDetails }) {


    const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });
    const typographyStyle = {
        fontSize: {
            xs: '0.8rem',
            sm: '0.9rem',
            md: '1.0rem',
            lg: '1.1rem'
        }
    };

    const editRide = (infoParam) => {
        axios.put(`http://192.168.0.9:5239/ride/info/update/${rideDetails.id}`, infoParam)
        .then((response) => {
            setRideDetails(response.data);
            setSnackbar({ open: true, message: 'Ride changed successfully!', severity: 'success' });

        })
        .catch((error) => {
            console.log(error);
            setSnackbar({ open: true, message: 'Failed to change ride!', severity: 'error' });
          });
      };
    
    const handleSubmit = (infoParam) => {
        editRide(infoParam);
        setTimeout(() => {
            setOpen(false);
            window.location.reload();
        }, 1000);
    }


    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

  return (
        <>
            <Dialog open={open} onClose={() => setOpen(false)} maxWidth="md" fullWidth>
                <DialogTitle sx={{ bgcolor: 'primary.main', color: 'primary.contrastText', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                    <Typography sx={typographyStyle}></Typography>
                    <IconButton onClick={() => setOpen(false)} sx={{ color: 'primary.contrastText' }}>
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent >
                    <RideForm rideData={rideDetails}  handleSubmit={handleSubmit} titleText={"Edit Ride"}/>
                </DialogContent>
            </Dialog>
            <CustomSnackbar 
                open={snackbar.open} 
                message={snackbar.message} 
                severity={snackbar.severity} 
                onClose={handleCloseSnackbar} 
            />
        </>
  )
}
