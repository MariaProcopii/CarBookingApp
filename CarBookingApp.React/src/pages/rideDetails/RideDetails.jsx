import { Dialog, DialogTitle, DialogContent, Typography, Container, Box, Paper, Grid, IconButton,
         Accordion, AccordionSummary, AccordionDetails } from "@mui/material";
import OwnerDetails from "../../components/ownerDetails/OwnerDetails";
import PassengerDetails from "../../components/passengerDetails/PassengerDetails";
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import CloseIcon from '@mui/icons-material/Close';
import { getDateFromISO } from "../../utils/DateTimeUtils";
import { styled } from '@mui/system';
import React, { useEffect, useState } from 'react';
import axios from 'axios';

export default function RideDetails(props) {
    const {openRideDetails, setOpenRideDetails, rideId} = props;
    const [rideDetails, setRideDetails] = useState();

    const fetchRideDetails = () => {
        console.log()
        axios.get(`http://localhost:5239/ride/details/${rideId}`)
          .then((response) => {
            setRideDetails(response.data);
            console.log(rideDetails.rideDetail)
          })
          .catch((error) => {
              console.log(error);
            });
      };

      useEffect(() => {
        if (openRideDetails) {
            fetchRideDetails();
        }
    }, [openRideDetails]);

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

    return (
        <>
        {
            rideDetails != undefined
            ?
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
                        <OwnerDetails owner={rideDetails?.owner} />
                        <Box mt={2}>
                            <PassengerDetails passengers={rideDetails.passengers} />
                        </Box>
                    </Container>
                </DialogContent>
            </Dialog>
            :
            <></>
        }
        </>
    );
}
