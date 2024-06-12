import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Box, Tab, Tabs, Paper } from '@mui/material';
import DriverInfoTab from '../../components/driverInfoTab/DriverInfoTab';
import UserInfoTab from '../../components/userInfoTab/UserInfoTab';
import EditUserDialog from '../../components/editUserDialog/EditUserDialog';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import { parseErrorMessages } from '../../utils/ErrorUtils';

export default function Profile() {
  const [backendErrors, setBackendErrors] = useState({});
  const [open, setOpen] = useState(false);
  const [userInfo, setUserInfo] = useState(null);
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

  const fetchUserInfo = () => {
      
    axios.get(`http://192.168.0.9:5239/user/info/${claims.nameidentifier}`)
      .then((response) => {
          setUserInfo(response.data);
          console.log(response.data);
      })
      .catch((error) => {
          const { data } = error.response;
          setBackendErrors(parseErrorMessages(data.Message));
        });
  };

  useEffect(() => {
    setTimeout(() => {
      fetchUserInfo();
    }, 10);
  }, []);

  const handleSave = (updatedInfo) => {
    axios.put(`http://192.168.0.9:5239/user/info/update/${claims.nameidentifier}`, updatedInfo)
      .then(response => {
        setOpen(false);
        setUserInfo(updatedInfo);
        setToken(response.data);
      })
      .catch((error) => {
        const { data } = error.response;
        console.log(data.Message)
        setBackendErrors(parseErrorMessages(data.Message));
        console.log(backendErrors);
      });
  };

  const handleUpgrade = () => {
    // Logic to upgrade to a driver
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
            {userInfo && <UserInfoTab userInfo={userInfo} setOpen={setOpen}/>}
            {userInfo && <EditUserDialog open={open} setOpen={setOpen} userInfo={userInfo} handleSave={handleSave} backendErrors={backendErrors} setBackendErrors={setBackendErrors}/>}
          </>
        )}
        {tabValue === 1 && (
          <DriverInfoTab userInfo={userInfo} handleUpgrade={handleUpgrade}/>
        )}
      </Box>
    </Box>
  );
}
