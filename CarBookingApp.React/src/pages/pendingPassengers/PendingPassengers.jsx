import { Grid, Box, Container, Grow, Typography, CardMedia,  } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import axios from 'axios';
import Pagination from '@mui/material/Pagination';
import { useTheme } from '@mui/material/styles';
import PassengerDetails from '../../components/passengerDetails/PassengerDetails';
import Carousel from 'react-material-ui-carousel';
import { Typography, Box, Paper, Grid, Avatar, Divider } from '@mui/material';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import { styled } from '@mui/system';
import { calculateAge } from '../../utils/DateTimeUtils';
import getAvatarSrc from '../../utils/AvatarUtils';

export default function PendingPassengers() {

    const [users, setUsers] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);
    const theme = useTheme();

    // const fetchPendingRides = () => {
    //     axios.get(`http://192.168.0.9:5239/user/pending/${claims.nameidentifier}?PageNumber=${pageIndex}`)
    //         .then((response) => {
    //             setUsers(response.data.items);
    //             console.log(response.data.items);
    //             setTotalPages(response.data.totalPages);
    //         })
    //         .catch((error) => {
    //             const { data } = error.response;
    //             setBackendErrors(parseErrorMessages(data.Message));
    //         });
    // };

    useEffect(() => {
        setTimeout(() => {
            fetchPendingRides();
        }, 10);
    }, [pageIndex]);

    const typographyStyle = { 
        fontSize: {
            xs: '1.4rem',
            sm: '1.6rem',
            md: '1.8rem',
            lg: '2.0rem',
        },
        fontFamily: 'Raleway',
        marginBottom: 2 
    };

    const imageStyle = {
        width: {
            xs: '70%',
            sm: '50%',
            md: '40%',
            lg: '30%',
        },
        margin: '0 auto'
    };

    const getImagePath = () => {
        if (theme.palette.mode === 'dark') {
            return "src/assets/images/no-pending-passengers-dark.png";
        } else {
            return "src/assets/images/no-pending-passengers-light.png";
        }
    };

    return (
        <Container sx={{ display: 'flex', flexDirection: 'column', minHeight: '80vh', justifyContent: 'center' }}>
            <Box mb={5} />
            {users.length === 0 ? (
                <Box sx={{ textAlign: 'center', marginTop: 5 }}>
                    <Typography
                        color="textSecondary" 
                        sx={typographyStyle}
                    >
                        No pending users
                    </Typography>
                    <CardMedia
                        component="img"
                        image={getImagePath()}
                        alt="No users"
                        sx={imageStyle}
                    />
                </Box>
            ) : (
                <>
                    <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center' flexGrow={2}>
                        {/* {users.map((user) => (
                            <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                                <Grow in={true} timeout={500}>
                                    <div>
                                        <SmallRideCard ride={user} action={"unsubscribe"} />
                                    </div>
                                </Grow>
                            </Grid>
                        ))} */}
                    </Grid>
                    <Box mb={10} />
                    <Grid container direction='row' alignItems='center' justifyContent='center'>
                        <Pagination count={totalPages}
                            variant="outlined"
                            color="primary"
                            onChange={(e, value) => setPageIndex(value)}
                            sx={{
                                '& .MuiPaginationItem-root': {
                                    fontSize: {
                                        xs: '0.75rem',
                                        sm: '0.875rem',
                                        md: '1rem',
                                        lg: '1.25rem'
                                    },
                                    padding: {
                                        xs: '4px',
                                        sm: '6px',
                                        md: '8px',
                                        lg: '10px'
                                    }
                                }
                            }}
                        />
                    </Grid>
                </>
            )}
        </Container>
    );
}

import React from 'react';
import Carousel from 'react-material-ui-carousel';
import { Typography, Box, Paper, Grid, Avatar, Divider } from '@mui/material';
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import { styled } from '@mui/system';
import { calculateAge } from '../../utils/DateTimeUtils';
import getAvatarSrc from '../../utils/AvatarUtils';

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

const iconStyle = {
    fontSize: {
        xs: '1rem',
        sm: '1.2rem',
        md: '1.5rem',
        lg: '1.7rem'
    },
    marginRight: '8px'
};

function PassengerDetails({ passengers }) {
    return (
        <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }} variant="h6" gutterBottom>Passengers</Typography>
            <Carousel
                animation="slide"
                interval={4000}
            >
                {passengers.map((passenger) => (
                    <Box key={passenger.id} sx={{ p: 2 }}>
                        <Grid container spacing={2} alignItems="center">
                            <Grid item>
                                <Avatar sx={{ bgcolor: 'primary.main', width: 56, height: 56 }}
                                        src={getAvatarSrc(passenger.gender)}>
                                    {passenger.firstName.charAt(0)}{passenger.lastName.charAt(0)}
                                </Avatar>
                            </Grid>
                            <Grid item xs>
                                <DetailBox>
                                    <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Name:</Typography>
                                    <Typography sx={typographyStyle}>&nbsp;{passenger.firstName} {passenger.lastName}</Typography>
                                </DetailBox>
                                <DetailBox>
                                    <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Age:</Typography>
                                    <Typography sx={typographyStyle}>&nbsp;{calculateAge(passenger.dateOfBirth)}</Typography>
                                </DetailBox>
                                <Divider sx={{ my: 2 }} />
                                <DetailBox>
                                    <EmailIcon color="primary" sx={iconStyle} />
                                    <Typography sx={typographyStyle}>{passenger.email}</Typography>
                                </DetailBox>
                                <DetailBox>
                                    <PhoneIcon color="primary" sx={iconStyle} />
                                    <Typography sx={typographyStyle}>{passenger.phoneNumber}</Typography>
                                </DetailBox>
                            </Grid>
                        </Grid>
                    </Box>
                ))}
            </Carousel>
        </Paper>
    );
}

export default function RideDetailsCard({ userInfo, rideInfo }) {
    return (
        <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }} variant="h6" gutterBottom>User Information</Typography>
            <PassengerDetails passengers={[userInfo]} />
            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }} variant="h6" gutterBottom>Ride Information</Typography>
            <Box sx={{ p: 2 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item xs={12}>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>From:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{rideInfo.destinationFrom}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>To:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{rideInfo.destinationTo}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: 'bold' }}>Date of the Ride:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{new Date(rideInfo.dateOfTheRide).toLocaleString()}</Typography>
                        </DetailBox>
                    </Grid>
                </Grid>
            </Box>
        </Paper>
    );
}

