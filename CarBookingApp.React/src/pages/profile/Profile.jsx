import React, { useState } from 'react';
import { Box, Tab, Tabs, Paper } from '@mui/material';
import DriverInfoTab from '../../components/driverInfoTab/DriverInfoTab';
import UserInfoTab from '../../components/userInfoTab/UserInfoTab';
import EditUserDialog from '../../components/editUserDialog/EditUserDialog';

const user = {
  id: 19,
  firstName: "Maria",
  lastName: "Procopii",
  gender: "FEMALE",
  dateOfBirth: "2002-05-17T00:00:00",
  email: "mari.procopii@gmail.com",
  phoneNumber: "069889062",
  yearsOfExperience: 2,
  vehicleDetail: null,
};

export default function Profile() {
  const [open, setOpen] = useState(false);
  const [userInfo, setUserInfo] = useState(user);
  const [tabValue, setTabValue] = useState(0);

  const typographyStyle = {
    fontSize: {
      xs: '0.8rem',
      sm: '0.9rem',
      md: '1.0rem'
    }
  };

  const handleSave = () => {
    setOpen(false);
    // Logic to save the updated user info
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
            <UserInfoTab userInfo={userInfo} setOpen={setOpen}/>
            <EditUserDialog open={open} setOpen={setOpen} userInfo={userInfo} setUserInfo={setUserInfo} handleSave={handleSave}/>
          </>
        )}
        {tabValue === 1 && (
          <DriverInfoTab userInfo={userInfo} handleUpgrade={handleUpgrade}/>
        )}
      </Box>
    </Box>
  );
}
