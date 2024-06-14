import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Box, Tab, Tabs, Paper } from '@mui/material';
import DriverInfoTab from '../../components/driverInfoTab/DriverInfoTab';
import UserInfoTab from '../../components/userInfoTab/UserInfoTab';
import EditUserDialog from '../../components/editUserDialog/EditUserDialog';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import { parseErrorMessages } from '../../utils/ErrorUtils';
import EditDriverDialog from '../../components/editDriverDialog/EditDriverDialog';
import UpgradeUserDialog from '../../components/upgradeUserDialog/UpgradeUserDialog';

export default function Profile() {
  const [backendErrors, setBackendErrors] = useState({});
  const [openUserDialog, setOpenUserDialog] = useState(false);
  const [openDriverDialog, setOpenDriverDialog] = useState(false);
  const [openUpgradeDialog, setOpenUpgradeDialog] = useState(false);
  const [userInfo, setUserInfo] = useState(null);
  const [vehicleDetail, setVehicleDetail] = useState(null);
  const [tabValue, setTabValue] = useState(0);
  const { token, setToken } = useAuth();
  const claims = useTokenDecoder(token);

  const typographyStyle = {
    fontSize: {
      xs: '0.8rem',
      sm: '0.9rem',
      md: '1.0rem'
    }
  };

  const fetchInfo = () => {
      
    axios.get(`http://192.168.0.9:5239/user/info/${claims.nameidentifier}`)
      .then((response) => {
          setUserInfo(response.data);
          setVehicleDetail(response.data.vehicleDetail);
      })
      .catch((error) => {
          const { data } = error.response;
          setBackendErrors(parseErrorMessages(data.Message));
        });
  };

  useEffect(() => {
    setTimeout(() => {
      fetchInfo();
    }, 10);
  }, []);

  const handleUpdateUser = (updatedInfo) => {
    axios.put(`http://192.168.0.9:5239/user/info/update/${claims.nameidentifier}`, updatedInfo)
      .then(response => {
        console.log(updatedInfo);
        setOpenUserDialog(false);
        setUserInfo(updatedInfo);
        setToken(response.data);
      })
      .catch((error) => {
        const { data } = error.response;
        console.log(data.Message);
        setBackendErrors(parseErrorMessages(data.Message));
        console.log(backendErrors);
      });
  };

  const handleTabChange = (event, newValue) => {
    setTabValue(newValue);
  };

  return (
    <Box sx={{ mt: 5, display: 'flex', justifyContent: 'center' }}>
      <Box sx={{ width: '90%', maxWidth: 700 }}>
        <Paper square>
          <Tabs
            value={tabValue}
            indicatorColor="primary"
            textColor="primary"
            onChange={handleTabChange}
            aria-label="profile tabs"
          >
            <Tab label="User Info" sx={typographyStyle}/>
            <Tab label="Driver Info" sx={typographyStyle}/>
          </Tabs>
        </Paper>
        {tabValue === 0 && (
          <>
            {userInfo && <UserInfoTab userInfo={userInfo} setOpen={setOpenUserDialog}/>}
            {userInfo && <EditUserDialog 
                            open={openUserDialog} 
                            setOpen={setOpenUserDialog} 
                            userInfo={userInfo} 
                            handleSave={handleUpdateUser} 
                            backendErrors={backendErrors}
                            setBackendErrors={setBackendErrors}
                          />
            }
          </>
        )}
        {tabValue === 1 && (
          <>
          <DriverInfoTab 
            vehicleDetail={vehicleDetail} 
            setOpenEdit={setOpenDriverDialog} 
            setOpenUpgrade ={setOpenUpgradeDialog}
          />

          {vehicleDetail && <EditDriverDialog 
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