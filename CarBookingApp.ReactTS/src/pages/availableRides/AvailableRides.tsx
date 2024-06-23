import Ride from "../../components/ride/Ride";
import SearchBar from "../../components/searchBar/SearchBar";
import Pagination from "@mui/material/Pagination";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import ManageSearchIcon from "@mui/icons-material/ManageSearch";
import { TRide } from "../../models/Ride";
import { TOwner } from "../../models/Owner";
import { useEffect, useState } from "react";
import { Grid, Box, Container, useMediaQuery, Button, Dialog, DialogContent, Grow } from "@mui/material";
import useAuth from "../../context/auth/UseAuth";
import { useTokenDecoder } from "../../utils/TokenUtils";
import { useTheme } from "@mui/material/styles";
import RideReviewDialog from "../../components/rideReviewDialog/RideReviewDialog";
import CustomSnackbar from "../../components/customSnackbar/CustomSnackbar";
import { useLocation } from 'react-router-dom';

export default function AvailableRides() {
    const containerStyle = {
        display: "flex",
        flexDirection: "column",
        minHeight: "80vh",
        justifyContent: "center",
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
        },
    };
    const buttonStyle = {
        mr: 2,
        fontSize: {
            xs: "0.6rem",
            sm: "0.7rem",
            md: "0.8rem",
            lg: "0.9rem",
        },
    };

    const [rides, setRides] = useState<TRide[]>([] as TRide[]);
    const [completedRidesOwner, setCompletedRidesOwner] = useState<TOwner[]>([] as TOwner[]);
    const [pageIndex, setPageIndex] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [open, setOpen] = useState(false);
    const [openRideReview, setOpenRideReview] = useState<boolean[]>([] as boolean[]);
    const location = useLocation();
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });

    const theme = useTheme();
    const isSmallScreen = useMediaQuery(theme.breakpoints.down("sm"));

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { fetchRides, fetchCompletedRidesOwnerInfo } = RideService(instance);

    useEffect(() => {
        setTimeout(() => {
            fetchRides(
                claims.nameidentifier as string,
                pageIndex,
                setRides,
                setTotalPages
            );
        }, 10);
    }, [pageIndex]);

    useEffect(() => {
        setTimeout(() => {
            fetchCompletedRidesOwnerInfo(
                claims.nameidentifier as string,
                (owners) => {
                    setCompletedRidesOwner(owners);
                    setOpenRideReview(new Array(owners.length).fill(true));
                }
            );
        }, 30);
    }, [pageIndex]);

    useEffect(() => {
        if (location.state) {
        setSnackbar({
            open: location.state.open,
            message: location.state.message,
            severity: location.state.severity
        });
        }
    }, [location.state]);

    const handleClickOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleCloseRideReview = (index: number) => {
        setOpenRideReview((prev) => {
            const newOpenRideReview = [...prev];
            newOpenRideReview[index] = false;
            return newOpenRideReview;
        });
    };

    return (
        <Container sx={containerStyle}>
            <Grid container direction="row" alignItems="center" justifyContent="center">
                {isSmallScreen ? (
                    <Grid item>
                        <Button variant="contained"
                            startIcon={<ManageSearchIcon fontSize="large" htmlColor="black" />}
                            sx={buttonStyle}
                            onClick={handleClickOpen}
                        >
                            Search
                        </Button>
                        <Dialog open={open} onClose={handleClose} fullWidth maxWidth="xs">
                            <DialogContent>
                                <SearchBar
                                    setRides={setRides}
                                    setTotalPages={setTotalPages}
                                    handleClose={handleClose}
                                />
                            </DialogContent>
                        </Dialog>
                    </Grid>
                ) : (
                    <Grid item>
                        <Box sx={{ mr: 6 }}>
                            <SearchBar
                                setRides={setRides}
                                setTotalPages={setTotalPages}
                                handleClose={handleClickOpen}
                            />
                        </Box>
                    </Grid>
                )}
            </Grid>
            <Box mb={5} />
            <Grid
                container
                spacing={5}
                direction="row"
                wrap="wrap"
                alignItems="center"
                justifyContent="center"
                flexGrow={2}
            >
                {rides.map((ride) => (
                    <Grid item xs={6} sm={5} md={4} lg={3} key={ride.id}>
                        <Grow in={true} timeout={500}>
                            <div>
                                <Ride ride={ride} action={"book"} />
                            </div>
                        </Grow>
                    </Grid>
                ))}
            </Grid>
            {completedRidesOwner.map((owner, index) => (
                <RideReviewDialog 
                    key={owner.id}
                    open={openRideReview[index]}
                    handleClose={() => handleCloseRideReview(index)}
                    owner={owner}
                />
            ))}
            <Box mb={10} />
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
        </Container >
    )
}