import PassengerDetails from "../passengerDetails/PassengerDetails";
import { TInfoItem } from "../../models/InfoItem";
import {
    getTimeFromISO, getDateFromISO
} from "../../utils/DateTimeUtils";
import {
    Grid, Box, Paper,
    Typography, Button
} from "@mui/material";

interface Props {
    userInfo: any;
    rideInfo: any;
    setSnackbar: any;
    approvePassenger: any;
    rejectPassenger: any;
}

export default function PendingPassengerDetail({ userInfo, rideInfo, setSnackbar, approvePassenger, rejectPassenger }: Props) {
    const paperStyle = {
        p: 2, 
        mb: 1, 
        borderRadius: 2, 
        minWidth: "300px", 
        maxHeight: "800px",
    };
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
            lg: "1.1rem",
        }
    };
    const buttonStyle = {
        fontSize: {
            xs: "0.6rem",
            sm: "0.6rem",
            md: "0.9rem",
        }
    };
    const spacingStyle = {
        display: "flex",
        justifyContent: "space-between",
        paddingLeft: "6%",
        paddingRight: "6%",
    };

    const InfoItem = ({ label, value }: TInfoItem) => (
        <Box sx={spacingStyle}>
            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }} variant="body1" component="span">{label}:</Typography>
            <Typography sx={typographyStyle} variant="body1" component="span">{value}</Typography>
        </Box>
    );

    const handleApprovePassenger = () => {
        const infoParam = {
            rideId: rideInfo.id,
            passengerId: userInfo.id
        }
        approvePassenger(infoParam, setSnackbar);

        setTimeout(() => {
            window.location.reload();
        }, 1000);
    };

    const handleRejectPassenger = () => {
        const infoParam = {
            rideId: rideInfo.id,
            passengerId: userInfo.id
        }

        rejectPassenger(infoParam, setSnackbar);

        setTimeout(() => {
            window.location.reload();
        }, 1000);
    };

    return (
        <Paper elevation={3} sx={paperStyle}>
            <Typography sx={{ ...typographyStyle, fontWeight: "bold", ml: 2, fontFamily: "Raleway" }} gutterBottom>User Information</Typography>
            <PassengerDetails passengers={[userInfo]} />
            <Typography sx={{ ...typographyStyle, fontWeight: "bold", ml: 2, fontFamily: "Raleway" }} gutterBottom>Ride Information</Typography>
            <Box sx={{ p: 2 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={12}>
                        <InfoItem label="From" value={rideInfo.destinationFrom} />
                        <InfoItem label="To" value={rideInfo.destinationTo} />
                        <InfoItem label="Date" value={getDateFromISO(rideInfo.dateOfTheRide)} />
                        <InfoItem label="Time" value={getTimeFromISO(rideInfo.dateOfTheRide)} />
                    </Grid>
                </Grid>
            </Box>
            <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    sx={buttonStyle}
                    onClick={handleApprovePassenger}
                >
                    Approve Passenger
                </Button>
            </Box>
            <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                <Button
                    fullWidth
                    variant="contained"
                    color="primary"
                    sx={buttonStyle}
                    onClick={handleRejectPassenger}
                >
                    Reject Passenger
                </Button>
            </Box>
        </Paper>
    );
}