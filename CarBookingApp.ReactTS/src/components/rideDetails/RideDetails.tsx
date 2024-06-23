import dayjs from "dayjs";
import OwnerDetails from "../ownerDetails/OwnerDetails";
import PassengerDetails from "../passengerDetails/PassengerDetails";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import CloseIcon from "@mui/icons-material/Close";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import EditRideDialog from "../editRideDialog/EditRideDialog";
import CustomSnackbar from "../../components/customSnackbar/CustomSnackbar";
import { styled } from "@mui/system";
import { TRideDetails } from "../../models/RideDetails";
import { getDateFromISO } from "../../utils/DateTimeUtils";
import { useEffect, useState } from "react";
import {
    Dialog, DialogTitle, DialogContent, Typography,
    Container, Box, Paper, Grid, IconButton, Button,
    Accordion, AccordionSummary, AccordionDetails
} from "@mui/material";
import useAuth from "../../context/auth/UseAuth";
import { hasRole, useTokenDecoder } from "../../utils/TokenUtils";
import TipDialog from "../tipDialog/TipDialog";

interface Props {
    openRideDetails: boolean;
    rideId: number;
    setOpenRideDetails: (openRideDetails: boolean) => void;
    action?: any;
}

export default function RideDetails({ openRideDetails, setOpenRideDetails, rideId, action }: Props) {
    const dialogTitleStyle = {
        display: "flex",
        bgcolor: "primary.main",
        color: "primary.contrastText",
        justifyContent: "space-between",
        alignItems: "center"
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
            md: "0.9rem"
        }
    };
    const DetailBox = styled(Box)(({ theme }) => ({
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(1),
    }));

    const [rideDetails, setRideDetails] = useState<TRideDetails | null>(null);
    const [isEditBDisabled, setEditBDisabled] = useState(false);
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });
    const [isUnsubscribeBDisabled, setUnsubscribeBDisabled] = useState(false);
    const [isDeleteBDisabled, setDeleteBDisabled] = useState(false);
    const [openEditRide, setOpenEditRide] = useState(false);
    const [openTipDialog, setOpenTipDialog] = useState(false);

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const {
        fetchRideDetails,
        bookRide,
        unsubscribeFromRide,
        deleteRide,
    } = RideService(instance);

    useEffect(() => {
        if (openRideDetails) {
            fetchRideDetails(rideId, setRideDetails);
        }
    }, [openRideDetails, rideId]);

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    const handleBookRide = async () => {
        const infoParam = {
            RideId: rideId,
            PassengerId: claims.nameidentifier
        }

        await bookRide(infoParam, setSnackbar);
        setEditBDisabled(true);

        setTimeout(() => {
            setOpenRideDetails(false);
            window.location.reload();
        }, 1000);
    };

    const handleUnsubscribeFromRide = async () => {
        const infoParam = {
            rideId: rideId,
            passengerId: claims.nameidentifier
        }

        await unsubscribeFromRide(infoParam, setSnackbar);
        setUnsubscribeBDisabled(true);

        setTimeout(() => {
            setOpenRideDetails(false);
            window.location.reload();
        }, 1000);
    }
    
    const handleDeleteRide = () => {
        deleteRide(rideId, setSnackbar);
        setDeleteBDisabled(true);

        setTimeout(() => {
            setOpenRideDetails(false);
            window.location.reload();
          }, 1000);
    }

    const handleOpenEditRideDialog = () => {
        setOpenRideDetails(false);
        setOpenEditRide(true)
    }

    const handleOpenTipDialog = () => {
        setOpenTipDialog(true);
    };

    return (
        <>
            {
                rideDetails != null
                    ?
                    <>
                        <Dialog open={openRideDetails} onClose={() => setOpenRideDetails(false)} maxWidth="md" fullWidth>
                            <DialogTitle sx={dialogTitleStyle}>
                                <Typography sx={typographyStyle}>Ride Details</Typography>
                                <IconButton onClick={() => setOpenRideDetails(false)} sx={{ color: "primary.contrastText" }}>
                                    <CloseIcon />
                                </IconButton>
                            </DialogTitle>
                            <DialogContent >
                                <Container sx={{ mt: 2 }}>
                                    <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
                                        <Grid container spacing={2}>
                                            <Grid item xs={12} sm={6}>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Date of the Ride:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{getDateFromISO(rideDetails.dateOfTheRide)}</Typography>
                                                </DetailBox>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>From:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{rideDetails.destinationFrom}</Typography>
                                                </DetailBox>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>To:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{rideDetails.destinationTo}</Typography>
                                                </DetailBox>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Total Seats:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{rideDetails.totalSeats}</Typography>
                                                </DetailBox>
                                            </Grid>
                                            <Grid item xs={12} sm={6}>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Pick Up Spot:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{rideDetails.rideDetail.pickUpSpot}</Typography>
                                                </DetailBox>
                                                <DetailBox>
                                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Price:</Typography>
                                                    <Typography sx={typographyStyle}>&nbsp;{rideDetails.rideDetail.price} lei</Typography>
                                                </DetailBox>
                                                <Accordion>
                                                    <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                                                        <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Facilities</Typography>
                                                    </AccordionSummary>
                                                    <AccordionDetails>
                                                        <Typography sx={typographyStyle}>{rideDetails.rideDetail.facilities.join(", ")}</Typography>
                                                    </AccordionDetails>
                                                </Accordion>
                                            </Grid>
                                        </Grid>
                                    </Paper>
                                    <OwnerDetails owner={rideDetails.owner} />
                                    <Box mt={2}>
                                        <PassengerDetails passengers={rideDetails.passengers} />
                                    </Box>
                                    {hasRole(claims, "Driver") && action === "edit" && (
                                        <>
                                            <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                                                <Button
                                                    fullWidth
                                                    variant="contained"
                                                    color="primary"
                                                    sx={buttonStyle}
                                                    onClick={handleOpenEditRideDialog}
                                                >
                                                    Edit ride
                                                </Button>
                                            </Box>
                                            <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                                                <Button
                                                    fullWidth
                                                    variant="contained"
                                                    color="primary"
                                                    disabled={isDeleteBDisabled}
                                                    sx={buttonStyle}
                                                    onClick={handleDeleteRide}
                                                >
                                                    Delete ride
                                                </Button>
                                            </Box>
                                        </>
                                    )}
                                    {action === "book" && (
                                        <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                                            <Button
                                                fullWidth
                                                variant="contained"
                                                color="primary"
                                                disabled={isEditBDisabled}
                                                sx={buttonStyle}
                                                onClick={handleBookRide}
                                            >
                                                Book Ride
                                            </Button>
                                        </Box>
                                    )}
                                    {action === "unsubscribe" && (
                                        <Box sx={{ display: "flex", justifyContent: "space-between", mt: 2 }}>
                                            <Button
                                                fullWidth
                                                variant="contained"
                                                color="primary"
                                                disabled={isUnsubscribeBDisabled}
                                                sx={buttonStyle}
                                                onClick={handleUnsubscribeFromRide}
                                            >
                                                Unsubscribe
                                            </Button>
                                        </Box>
                                    )}
                                    {action === "tip" && (
                                        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
                                            <Button
                                                fullWidth
                                                variant="contained"
                                                color="primary"
                                                sx={buttonStyle}
                                                onClick={handleOpenTipDialog}
                                            >
                                                Leave a Tip
                                            </Button>
                                        </Box>
                                    )}
                                </Container>
                            </DialogContent>
                            <CustomSnackbar
                                open={snackbar.open}
                                message={snackbar.message}
                                severity={snackbar.severity}
                                onClose={handleCloseSnackbar}
                            />
                        </Dialog>
                        {rideDetails &&
                            <EditRideDialog
                                open={openEditRide}
                                setOpen={setOpenEditRide}
                                rideDetails={{ ...rideDetails, dateOfTheRide: dayjs(rideDetails.dateOfTheRide) }}
                                setRideDetails={setRideDetails}
                            />
                        }
                        <TipDialog
                            open={openTipDialog}
                            onClose={() => setOpenTipDialog(false)}
                            driverEmail={"sb-yi8si31297448@business.example.com"}
                            tipperEmail={"sb-hg6lf31201930@personal.example.com"}
                        />
                    </>
                    :
                    <></>
            }
        </>
    );
}