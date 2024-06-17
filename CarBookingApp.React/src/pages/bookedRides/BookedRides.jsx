import { Grid, Box, Container, Grow, Typography, CardMedia } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import axios from 'axios';
import Pagination from '@mui/material/Pagination';
import SmallRideCard from '../../components/smallRideCard/SmallRideCard';
import { useTheme } from '@mui/material/styles';

export default function BookedRides() {

    const [rides, setRides] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const { token } = useAuth();
    const claims = useTokenDecoder(token);
    const theme = useTheme();

    const fetchRides = () => {
        axios.get(`http://192.168.0.9:5239/ride/booked/${claims.nameidentifier}?PageNumber=${pageIndex}`)
            .then((response) => {
                setRides(response.data.items);
                setTotalPages(response.data.totalPages);
            })
            .catch((error) => {
                const { data } = error.response;
                setBackendErrors(parseErrorMessages(data.Message));
            });
    };

    useEffect(() => {
        setTimeout(() => {
            fetchRides();
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
            return "src/assets/images/no-booked-rides-dark.png";
        } else {
            return "src/assets/images/no-booked-rides-light.png";
        }
    };

    return (
        <Container sx={{ display: 'flex', flexDirection: 'column', minHeight: '80vh', justifyContent: 'center' }}>
            <Box mb={5} />
            {rides.length === 0 ? (
                <Box sx={{ textAlign: 'center', marginTop: 5 }}>
                    <Typography 
                        variant="poster" 
                        color="textSecondary" 
                        sx={typographyStyle}
                    >
                        No booked rides
                    </Typography>
                    <CardMedia
                        component="img"
                        image={getImagePath()}
                        alt="No rides"
                        sx={imageStyle}
                    />
                </Box>
            ) : (
                <>
                    <Grid container spacing={5} direction='row' wrap='wrap' alignItems='center' justifyContent='center' flexGrow={2}>
                        {rides.map((ride) => (
                            <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                                <Grow in={true} timeout={500}>
                                    <div>
                                        <SmallRideCard ride={ride} action={"unsubscribe"} />
                                    </div>
                                </Grow>
                            </Grid>
                        ))}
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
