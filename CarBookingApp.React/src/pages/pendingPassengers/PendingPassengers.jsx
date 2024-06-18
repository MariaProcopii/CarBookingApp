import { Grid, Box, Container, Grow, Typography, CardMedia, Paper, Avatar, Divider } from '@mui/material';
import { useEffect, useState } from 'react';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import axios from 'axios';
import Pagination from '@mui/material/Pagination';
import { useTheme } from '@mui/material/styles';
import PendingPassengerDetail from '../../components/pendingPassengerDetail/PendingPassengerDetail';
import CustomSnackbar from '../../components/customSnackbar/CustomSnackbar';

export default function PendingPassengers() {

    const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });
    const [pendingUsersInfo, setPendingUsersInfo] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);
    const theme = useTheme();

    const fetchPendingRides = () => {
        axios.get(`http://192.168.0.9:5239/user/pending/${claims.nameidentifier}?PageNumber=${pageIndex}`)
            .then((response) => {
                setPendingUsersInfo(response.data.items);
                console.log(response.data.items);
                console.log(response.data.items[0].userInfo);
                console.log(response.data.items[0].rideInfo);
                setTotalPages(response.data.totalPages);
            })
            .catch((error) => {
                const { data } = error.response;
                setBackendErrors(parseErrorMessages(data.Message));
            });
    };

    const approvePassengerForRide = (infoParam) => {
        axios.put("http://192.168.0.9:5239/user/pending/approve", infoParam)
          .then((response) => {
            setSnackbar({ open: true, message: 'Passener approved successfully!', severity: 'success' });
          })
          .catch((error) => {
              console.log(error);
              setSnackbar({ open: true, message: 'Failed to approve passenger from ride!', severity: 'error' });
            });
      };

    const rejectPassengerFromRide = (infoParam) => {
    axios.put("http://192.168.0.9:5239/user/pending/reject", infoParam)
        .then((response) => {
        setSnackbar({ open: true, message: 'Passenger rejected successfully!', severity: 'success' });
        })
        .catch((error) => {
            console.log(error);
            setSnackbar({ open: true, message: 'Failed to reject passenger from ride!', severity: 'error' });
        });
    };


    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

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
        <Container sx={{ display: 'flex', flexDirection: 'column', minHeight: '80vh', minWidth: '90vw', justifyContent: 'center', }}>
            {pendingUsersInfo.length === 0 ? (
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
                    <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center'>
                        {pendingUsersInfo.map((user) => (
                            <Grid item xs={12} sm={9} md={7} lg={6} key={user.userInfo.Id}>
                                <Grow in={true} timeout={500}>
                                    <div>
                                        <PendingPassengerDetail userInfo={user.userInfo} 
                                                                rideInfo={user.rideInfo}
                                                                approvePassenger={approvePassengerForRide}
                                                                rejectPassenger={rejectPassengerFromRide}
                                        />
                                    </div>
                                </Grow>
                            </Grid>
                        ))}
                    </Grid>
                    <Box mb={5} />
                    <Grid container direction='row' alignItems='center' justifyContent='center'>
                        <Pagination
                            count={totalPages}
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
            <CustomSnackbar 
                open={snackbar.open} 
                message={snackbar.message} 
                severity={snackbar.severity} 
                onClose={handleCloseSnackbar} 
            />
        </Container>
    );
}