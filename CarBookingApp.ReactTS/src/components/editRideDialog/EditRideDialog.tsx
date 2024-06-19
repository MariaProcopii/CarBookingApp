import CloseIcon from "@mui/icons-material/Close";
import RideForm from "../rideForm/RideForm";
import CustomSnackbar from "../customSnackbar/CustomSnackbar";
import { useState } from "react";
import { 
    Dialog, DialogTitle, 
    DialogContent, Typography, 
    IconButton 
} from "@mui/material";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";

interface Props {
    open: any;
    setOpen: any;
    rideDetails: any;
    setRideDetails: any;
}

export default function EditRideDialog({ open, setOpen, rideDetails, setRideDetails }: Props) {
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
            lg: "1.1rem"
        }
    };

    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });

    const { instance } = useAPI();
    const { editRide } = RideService(instance);

    const handleSubmit = async (infoParam: any) => {
        editRide(
            rideDetails.id,
            infoParam,
            setRideDetails,
            setSnackbar,
        );
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
                <DialogTitle sx={{ bgcolor: "primary.main", color: "primary.contrastText", display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                    <Typography sx={typographyStyle}></Typography>
                    <IconButton onClick={() => setOpen(false)} sx={{ color: "primary.contrastText" }}>
                        <CloseIcon />
                    </IconButton>
                </DialogTitle>
                <DialogContent >
                    <RideForm 
                        rideData={rideDetails} 
                        handleSubmit={handleSubmit} 
                        setSnackbar={setSnackbar}
                        titleText={"Edit Ride"} 
                    />
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
