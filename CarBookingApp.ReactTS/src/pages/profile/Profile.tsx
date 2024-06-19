import useAPI from "../../context/api/UseAPI";
import useAuth from "../../context/auth/UseAuth";
import DriverInfoTab from "../../components/driverInfoTab/DriverInfoTab";
import UserInfoTab from "../../components/userInfoTab/UserInfoTab";
import EditUserDialog from "../../components/editUserDialog/EditUserDialog";
import EditDriverDialog from "../../components/editDriverDialog/EditDriverDialog";
import UpgradeUserDialog from "../../components/upgradeUserDialog/UpgradeUserDialog";
import UserService from "../../services/userService/UserService";
import { useTokenDecoder } from "../../utils/TokenUtils";
import { DetailsParams } from "../../services/userService/types";
import { VehicleDetailsParams } from "../../services/rideService/types";
import { Box, Tab, Tabs, Paper } from "@mui/material";
import { useState, useEffect, SyntheticEvent } from "react";

export default function Profile() {
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem"
        }
    };

    const [openUserDialog, setOpenUserDialog] = useState(false);
    const [openDriverDialog, setOpenDriverDialog] = useState(false);
    const [openUpgradeDialog, setOpenUpgradeDialog] = useState(false);
    const [userInfo, setUserInfo] = useState<DetailsParams | null>(null);
    const [vehicleDetail, setVehicleDetail] = useState<VehicleDetailsParams | null>(null);
    const [tabValue, setTabValue] = useState(0);

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { fetchInfo } = UserService(instance);

    useEffect(() => {
        setTimeout(() => {
            fetchInfo(
                claims.nameidentifier as string,
                setUserInfo,
                setVehicleDetail,
            );
        }, 10);
    }, []);

    const handleTabChange = (
        _: SyntheticEvent<Element, Event>, 
        newValue: number
    ) => {
        setTabValue(newValue);
    };

    return (
        <Box sx={{ mt: 5, display: "flex", justifyContent: "center" }}>
            <Box sx={{ width: "90%", maxWidth: 700 }}>
                <Paper square>
                    <Tabs
                        value={tabValue}
                        indicatorColor="primary"
                        textColor="primary"
                        onChange={handleTabChange}
                        aria-label="profile tabs"
                    >
                        <Tab label="User Info" sx={typographyStyle} />
                        <Tab label="Driver Info" sx={typographyStyle} />
                    </Tabs>
                </Paper>
                {tabValue === 0 && (
                    <>
                        {userInfo &&
                            <UserInfoTab
                                userInfo={userInfo}
                                setOpen={setOpenUserDialog}
                            />
                        }
                        {userInfo &&
                            <EditUserDialog
                                open={openUserDialog}
                                setOpen={setOpenUserDialog}
                                userInfo={userInfo}
                                setUserInfo={setUserInfo}
                            />
                        }
                    </>
                )}
                {tabValue === 1 && (
                    <>
                        <DriverInfoTab
                            vehicleDetail={vehicleDetail}
                            setOpenEdit={setOpenDriverDialog}
                            setOpenUpgrade={setOpenUpgradeDialog}
                        />

                        {vehicleDetail &&
                            <EditDriverDialog
                                open={openDriverDialog}
                                setOpen={setOpenDriverDialog}
                                vehicleDetail={vehicleDetail}
                                setVehicleDetail={setVehicleDetail}
                            />
                        }
                        <UpgradeUserDialog
                            open={openUpgradeDialog}
                            setOpen={setOpenUpgradeDialog}
                            setVehicleDetail={setVehicleDetail}
                            userInfo={userInfo}
                            setUserInfo={setUserInfo}
                        />
                    </>
                )}
            </Box>
        </Box>
    );
}