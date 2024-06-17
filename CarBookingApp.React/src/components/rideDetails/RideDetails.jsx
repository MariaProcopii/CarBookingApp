import { Dialog, DialogTitle, DialogContent, Typography, Container, Box, Paper, Grid, IconButton,
         Accordion, AccordionSummary, AccordionDetails, Button} from "@mui/material";
import OwnerDetails from "../../components/ownerDetails/OwnerDetails";
import PassengerDetails from "../../components/passengerDetails/PassengerDetails";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import CloseIcon from '@mui/icons-material/Close';
import { getDateFromISO } from "../../utils/DateTimeUtils";
import { styled } from '@mui/system';
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useTokenDecoder, hasRole } from '../../utils/TokenUtils';
import { useAuth } from '../provider/AuthProvider';
import CustomSnackbar from '../../components/customSnackbar/CustomSnackbar';
import RideForm from "../rideForm/RideForm";
import dayjs from 'dayjs';
import EditRideDialog from "../editRideDialog/EditRideDialog";

export default function RideDetails(props) {
    const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });
    const {openRideDetails, setOpenRideDetails, rideId} = props;
    const [isEditBDisabled, setEditBDisabled] = useState(false);
    const [isUnsubscribeBDisabled, setUnsubscribeBDisabled] = useState(false);
    const [rideDetails, setRideDetails] = useState();
    const [openEditRide, setOpenEditRide] = useState(false);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);

    const fetchRideDetails = () => {
        console.log()
        axios.get(`http://192.168.0.9:5239/ride/details/${rideId}`)
          .then((response) => {
            setRideDetails(response.data);
            console.log(response.data);
          })
          .catch((error) => {
              console.log(error);
            });
      };

      const bookRide = (infoParam) => {
        axios.post("http://192.168.0.9:5239/ride/info/book", infoParam)
          .then((response) => {
            setSnackbar({ open: true, message: 'Ride booked successfully!', severity: 'success' });
          })
          .catch((error) => {
              console.log(error);
              setSnackbar({ open: true, message: 'Failed to book ride!', severity: 'error' });
            });
      };

      const unsubscribeFromRide = (infoParam) => {
        axios.put("http://192.168.0.9:5239/ride/info/unsubscribe", infoParam)
          .then((response) => {
            setSnackbar({ open: true, message: 'Unsubscribed from ride successfully!', severity: 'success' });
          })
          .catch((error) => {
              console.log(error);
              setSnackbar({ open: true, message: 'Failed to unsubscribe from ride!', severity: 'error' });
            });
      };

      useEffect(() => {
        if (openRideDetails) {
            fetchRideDetails();
        }
    }, [openRideDetails]);

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    const handleBookRide = () => {
        const infoParam = {
            RideId : rideId,
            PassengerId : claims.nameidentifier
        }

        bookRide(infoParam);
        setEditBDisabled(true);

        setTimeout(() => {
            setOpenRideDetails(false);
            window.location.reload();
          }, 1000);
    }

    const handleUnsubscribeFromRide = () => {
        const infoParam = {
            rideId : rideId,
            passengerId : claims.nameidentifier
        }

        unsubscribeFromRide(infoParam);
        setUnsubscribeBDisabled(true);

        setTimeout(() => {
            setOpenRideDetails(false);
            window.location.reload();
          }, 1000);
    }

    const handleOpenEditRideDialog = () => {
        setOpenRideDetails(false);
        setOpenEditRide(true)
    }

    const typographyStyle = {
        fontSize: {
            xs: '0.8rem',
            sm: '0.9rem',
            md: '1.0rem',
            lg: '1.1rem'
        }
    };

    const DetailBox = styled(Box)(({ theme }) => ({
        display: 'flex',
        alignItems: 'center',
        padding: theme.spacing(1)
    }));

      const buttonStyle={  
        fontSize: {
          xs: '0.6rem',
          sm: '0.6rem',
          md: '0.9rem'
        }  
      };

    return (
        <>
        {
            rideDetails != null
            ?
            <>
                <Dialog open={openRideDetails} onClose={() => setOpenRideDetails(false)} maxWidth="md" fullWidth>
                    <DialogTitle sx={{ bgcolor: 'primary.main', color: 'primary.contrastText', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                        <Typography sx={typographyStyle}>Ride Details</Typography>
                        <IconButton onClick={() => setOpenRideDetails(false)} sx={{ color: 'primary.contrastText' }}>
                            <CloseIcon />
                        </IconButton>
                    </DialogTitle>
                    <DialogContent >
                        <Container sx={{ mt: 2 }}>
                            <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
                                <Grid container spacing={2}>
                                    <Grid item xs={12} sm={6}>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Date of the Ride:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{getDateFromISO(rideDetails?.dateOfTheRide)}</Typography>
                                        </DetailBox>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>From:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{rideDetails.destinationFrom}</Typography>
                                        </DetailBox>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>To:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{rideDetails.destinationTo}</Typography>
                                        </DetailBox>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Total Seats:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{rideDetails.totalSeats}</Typography>
                                        </DetailBox>
                                    </Grid>
                                    <Grid item xs={12} sm={6}>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Pick Up Spot:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{rideDetails.rideDetail.pickUpSpot}</Typography>
                                        </DetailBox>
                                        <DetailBox>
                                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Price:</Typography>
                                            <Typography sx={typographyStyle}>&nbsp;{rideDetails.rideDetail.price} lei</Typography>
                                        </DetailBox>
                                        <Accordion>
                                            <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                                                <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Facilities</Typography>
                                            </AccordionSummary>
                                            <AccordionDetails>
                                                <Typography sx={typographyStyle}>{rideDetails.rideDetail.facilities.join(', ')}</Typography>
                                            </AccordionDetails>
                                        </Accordion>
                                    </Grid>
                                </Grid>
                            </Paper>
                            <OwnerDetails owner={rideDetails.owner} />
                            <Box mt={2}>
                                <PassengerDetails passengers={rideDetails.passengers} />
                            </Box>
                            {hasRole(claims, "Driver") && props.action === "edit" && (
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
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
                            )}
                            {props.action === "book" && (
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
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
                            {props.action === "unsubscribe" && (
                                <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
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
                        rideDetails={{...rideDetails, dateOfTheRide: dayjs(rideDetails.dateOfTheRide)}}  
                        setRideDetails={setRideDetails}
                    />
                }
            </>
            :
            <></>
        }
        </>
    );
}
