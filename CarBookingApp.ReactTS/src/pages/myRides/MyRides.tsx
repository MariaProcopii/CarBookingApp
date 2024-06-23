import useTheme from "../../context/theme/UseTheme";
import useAuth from "../../context/auth/UseAuth";
import Pagination from "@mui/material/Pagination";
import SmallRideCard from "../../components/smallRideCard/SmallRideCard";
import { TRide } from "../../models/Ride";
import { useEffect, useState } from "react";
import { useTokenDecoder } from "../../utils/TokenUtils";
import {
    Grid, Box, Container,
    Grow, Typography, CardMedia
} from "@mui/material";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import CustomSnackbar from "../../components/customSnackbar/CustomSnackbar";
import { useLocation } from 'react-router-dom';

export default function MyRides() {
    const containerStyle = {
        display: "flex",
        flexDirection: "column",
        minHeight: "80vh",
        justifyContent: "center",
    };
    const typographyStyle = {
        fontSize: {
            xs: "1.4rem",
            sm: "1.6rem",
            md: "1.8rem",
            lg: "2.0rem",
        },
        fontFamily: "Raleway",
        marginBottom: 2,
    };
    const imageStyle = {
        width: {
            xs: "70%",
            sm: "50%",
            md: "40%",
            lg: "30%",
        },
        margin: "0 auto",
    };
    const paginationStyle = {
        "& .MuiPaginationItem-root": {
            fontSize: {
                xs: "0.75rem",
                sm: "0.875rem",
                md: "1rem",
                lg: "1.25rem",
            },
            padding: {
                xs: "4px",
                sm: "6px",
                md: "8px",
                lg: "10px",
            },
        }
    };

    const [rides, setRides] = useState<TRide[]>([] as TRide[]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const location = useLocation();
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });

    const { theme } = useTheme();

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { fetchCreatedRides } = RideService(instance);

    useEffect(() => {
        if (location.state) {
        setSnackbar({
            open: location.state.open,
            message: location.state.message,
            severity: location.state.severity
        });
        }
    }, [location.state]);

    useEffect(() => {
        setTimeout(() => {
            fetchCreatedRides(
                claims.nameidentifier as string,
                pageIndex,
                setRides,
                setTotalPages,
            );
        }, 10);
    }, [pageIndex]);

    const getImagePath = () => {
        if (theme?.palette.mode === "dark") {
            return "src/assets/images/no-booked-rides-dark.png";
        } else {
            return "src/assets/images/no-booked-rides-light.png";
        }
    };

    return (
        <Container sx={containerStyle}>
            {rides.length === 0 ? (
                <Box sx={{ textAlign: "center", marginTop: 5 }}>
                    <Typography
                        color="textSecondary"
                        sx={typographyStyle}
                    >
                        No Created Rides
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
                    <Grid container spacing={2} direction="row" wrap="wrap" alignItems="center" justifyContent="center" flexGrow={2}>
                        {rides.map((ride) => (
                            <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                                <Grow in={true} timeout={500}>
                                    <div>
                                        <SmallRideCard ride={ride} action={"tip"} />
                                    </div>
                                </Grow>
                            </Grid>
                        ))}
                    </Grid>
                    <Box mb={5} />
                    <Grid container direction="row" alignItems="center" justifyContent="center" >
                        <Pagination count={totalPages}
                            variant="outlined"
                            color="primary"
                            onChange={(_, value) => setPageIndex(value)}
                            sx={paginationStyle}
                        />
                    </Grid>
                    <CustomSnackbar 
                        open={snackbar.open} 
                        message={snackbar.message} 
                        severity={snackbar.severity} 
                        onClose={() => setSnackbar({ ...snackbar, open: false })}
                    />
                </>
            )}
        </Container>
    )
}