import useAuth from "../../context/auth/UseAuth";
import Pagination from "@mui/material/Pagination";
import PendingPassengerDetail from "../../components/pendingPassengerDetail/PendingPassengerDetail";
import CustomSnackbar from "../../components/customSnackbar/CustomSnackbar";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import UserService from "../../services/userService/UserService";
import { useTheme } from "@mui/material/styles";
import { useEffect, useState } from "react";
import { useTokenDecoder } from "../../utils/TokenUtils";
import {
    Grid, Box, Container,
    Grow, Typography, CardMedia,
} from "@mui/material";

export default function PendingPassengers() {
    const typographyStyle = {
        fontSize: {
            xs: "1.4rem",
            sm: "1.6rem",
            md: "1.8rem",
            lg: "2.0rem",
        },
        fontFamily: "Raleway",
        marginBottom: 2
    };
    const mainTypographyStyle = {
        fontSize: {
            xs: "1.4rem",
            sm: "1.6rem",
            md: "1.8rem",
            lg: "2.0rem",
        },
        fontFamily: "Raleway",
        marginBottom: 2,
        mr: {
            xs: 0,
            sm: 10,
            md: 20,
            lg: 10,
        },
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center"
    };
    const imageStyle = {
        width: {
            xs: "70%",
            sm: "50%",
            md: "40%",
            lg: "30%",
        },
        margin: "0 auto"
    };
    const paginationStyle = {
        "& .MuiPaginationItem-root": {
            fontSize: {
                xs: "0.75rem",
                sm: "0.875rem",
                md: "1rem",
                lg: "1.25rem"
            },
            padding: {
                xs: "4px",
                sm: "6px",
                md: "8px",
                lg: "10px"
            }
        }
    };

    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });
    const [pendingUsersInfo, setPendingUsersInfo] = useState([]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);

    const theme = useTheme();

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const {
        fetchUserPendingRides
    } = RideService(instance);
    const {
        approvePassengerForRide,
        rejectPassengerFromRide,
    } = UserService(instance);

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    useEffect(() => {
        setTimeout(() => {
            fetchUserPendingRides(
                claims.nameidentifier as string,
                pageIndex,
                setPendingUsersInfo,
                setTotalPages,
            );
        }, 10);
    }, [pageIndex]);


    const getImagePath = () => {
        if (theme.palette.mode === "dark") {
            return "src/assets/images/no-pending-passengers-dark.png";
        } else {
            return "src/assets/images/no-pending-passengers-light.png";
        }
    };

    return (
        <Container sx={{ display: "flex", flexDirection: "column", minHeight: "80vh", minWidth: "90vw", justifyContent: "center", }}>
            {pendingUsersInfo.length === 0 ? (
                <Box sx={{ textAlign: "center", marginTop: 5 }}>
                    <Typography
                        color="textSecondary"
                        sx={typographyStyle}
                    >
                        No Pending Passengers
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
                    <Typography
                        color="textSecondary"
                        sx={mainTypographyStyle}
                    >
                        Pending Passengers
                    </Typography>
                    <Grid container spacing={5} direction="row" wrap="wrap" alignItems="center" justifyContent="center">
                        {pendingUsersInfo.map((user: any) => (
                            <Grid item xs={12} sm={9} md={7} lg={6} key={user.userInfo.Id}>
                                <Grow in={true} timeout={500}>
                                    <div>
                                        <PendingPassengerDetail userInfo={user.userInfo}
                                            rideInfo={user.rideInfo}
                                            setSnackbar={setSnackbar}
                                            approvePassenger={approvePassengerForRide}
                                            rejectPassenger={rejectPassengerFromRide}
                                        />
                                    </div>
                                </Grow>
                            </Grid>
                        ))}
                    </Grid>
                    <Box mb={5} />
                    <Grid container direction="row" alignItems="center" justifyContent="center">
                        <Pagination
                            count={totalPages}
                            variant="outlined"
                            color="primary"
                            onChange={(_, value) => setPageIndex(value)}
                            sx={paginationStyle}
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