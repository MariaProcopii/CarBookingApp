import { Box, Typography, Button, Divider } from '@mui/material';
import DirectionsCarIcon from '@mui/icons-material/DirectionsCar';
import InfoIcon from '@mui/icons-material/Info';
import { styled } from '@mui/system';

export default function DriverInfoTab({ userInfo, handleUpgrade }) {

    const typographyStyle = {
        fontSize: {
          xs: '0.8rem',
          sm: '0.9rem',
          md: '1.0rem'
        }
      };

    const iconStyle = {
      fontSize: {
        xs: '1rem',
        sm: '1.2rem',
        md: '1.5rem',
        lg: '1.7rem'
      },
      marginRight: '8px'
    };
    
    const DetailBox = styled(Box)(({ theme }) => ({
      display: 'flex',
      alignItems: 'center',
      padding: theme.spacing(1)
    }));

  return (
    <Box sx={{ mt: 3, px: 3 }}>
    <Typography variant="h6" gutterBottom>
      Driver Detail
    </Typography>
    <Divider sx={{ mb: 2 }} />
    {userInfo.vehicleDetail ? (
      <>
        <DetailBox>
          <DirectionsCarIcon color="primary" sx={iconStyle} />
          <Typography sx={typographyStyle}>
            {userInfo.vehicleDetail.vehicle.vender} {userInfo.vehicleDetail.vehicle.model} ({userInfo.vehicleDetail.manufactureYear})
          </Typography>
        </DetailBox>
        <DetailBox>
          <InfoIcon color="primary" sx={iconStyle} />
          <Typography sx={typographyStyle}>{userInfo.vehicleDetail.registrationNumber}</Typography>
        </DetailBox>
      </>
    ) : (
      <>
        <Typography variant="body1" gutterBottom>
          By upgrading to a driver, you will be able to create rides and enjoy various benefits.
        </Typography>
        <Button variant="contained" color="primary" onClick={handleUpgrade}>
          Upgrade to Driver
        </Button>
      </>
    )}
  </Box>
  );
}