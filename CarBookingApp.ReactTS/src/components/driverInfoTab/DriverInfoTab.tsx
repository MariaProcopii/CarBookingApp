import useAPI from "../../context/api/UseAPI";
import useAuth from "../../context/auth/UseAuth";
import UserService from "../../services/userService/UserService";
import RideService from "../../services/rideService/RideService";
import InfoIcon from "@mui/icons-material/Info";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import CustomSnackbar from "../customSnackbar/CustomSnackbar";
import { styled } from "@mui/system";
import { TRide } from "../../models/Ride";
import { useState, useEffect } from "react";
import { useTokenDecoder, hasRole } from "../../utils/TokenUtils";
import { Box, Typography, Button, Divider } from "@mui/material";

interface Props {
    vehicleDetail: any;
    setOpenEdit: any;
    setOpenUpgrade: any;
}

export default function DriverInfoTab({ vehicleDetail, setOpenEdit, setOpenUpgrade }: Props) {
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
        },
    };
    const iconStyle = {
        fontSize: {
            xs: "1rem",
            sm: "1.2rem",
            md: "1.5rem",
            lg: "1.7rem",
        },
        marginRight: "8px",
    };
    const buttonStyleLeft = {
        borderRadius: {
            xs: "0 0 0 7px",
            sm: "0 0 0 10px",
            lg: "0 0 0 15px",
        },
        fontSize: {
            xs: "0.6rem",
            sm: "0.6rem",
            md: "0.9rem",
        },
    };
    const buttonStyleRight = {
        borderRadius: {
            xs: "0 0 7px 0",
            sm: "0 0 10px 0",
            lg: "0 0 15px 0",
        },
        fontSize: {
            xs: "0.6rem",
            sm: "0.6rem",
            md: "0.9rem",
        },
    };
    const buttonInfoStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.8rem",
            md: "0.9rem",
        },
    };
    const boxStyle = {
        display: "flex",
        justifyContent: "space-between",
        mt: 2
    };
    const DetailBox = styled(Box)(({ theme }) => ({
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(1),
    }));

    const [rides, setRides] = useState<TRide[]>([] as TRide[]);
    const [snackbar, setSnackbar] = useState({ open: false, message: "", severity: "success" });

    const { token, setToken } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { downgrade } = UserService(instance);
    const { fetchCreatedRides } = RideService(instance);

    useEffect(() => {
        setTimeout(() => {
            fetchCreatedRides(
                claims.nameidentifier as string,
                1,
                setRides,
                (_: number) => { },
            );
        }, 10);
    }, []);

    const downgradeUser = async () => {
        await downgrade(
            claims.nameidentifier as string,
            setToken
        );
    };

    const handleOpenEditDialog = () => {
        setOpenEdit(true);
    };

    const handleOpenUpgradeDialog = () => {
        setOpenUpgrade(true);
    };

    const handleDowngrade = () => {
        if (rides.length === 0) {
            downgradeUser();
        }
        else {
            setSnackbar(prev => ({ ...prev, open: false }))
            setTimeout(() => {
                setSnackbar({ open: true, message: 'First you need to finish or delete created rides.', severity: 'warning' });
            }, 100);
        }
    };

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    return (
        <Box sx={{ mt: 3, px: 3 }}>
            <Typography variant="h6" gutterBottom>
                Driver Detail
            </Typography>
            <Divider sx={{ mb: 2 }} />
            {
                hasRole(claims, "Driver") && vehicleDetail ? (
                    <>
                        <DetailBox>
                            <DirectionsCarIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>
                                {vehicleDetail.vehicle.vender} {vehicleDetail.vehicle.model} ({vehicleDetail.manufactureYear})
                            </Typography>
                        </DetailBox>
                        <DetailBox>
                            <InfoIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>{vehicleDetail.registrationNumber}</Typography>
                        </DetailBox>
                        <Box sx={boxStyle}>
                            <Button variant="contained" color="primary" sx={buttonStyleLeft} onClick={handleOpenEditDialog}>
                                Edit driver info
                            </Button>
                            <Button variant="contained" color="primary" sx={buttonStyleRight} onClick={handleDowngrade} >
                                Stop being a Driver
                            </Button>
                        </Box>
                        <CustomSnackbar
                            open={snackbar.open}
                            message={snackbar.message}
                            severity={snackbar.severity}
                            onClose={handleCloseSnackbar}
                        />
                    </>
                ) : (
                    <>
                        <Typography variant="body1" gutterBottom>
                            By upgrading to a driver, you will be able to create rides and enjoy various benefits.
                        </Typography>
                        <Button variant="contained" color="primary" sx={buttonInfoStyle} onClick={handleOpenUpgradeDialog}>
                            Upgrade to Driver
                        </Button>
                    </>
                )
            }
        </Box>
    );
}