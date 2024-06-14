import { Box, Typography, Button, Divider } from '@mui/material';
import DirectionsCarIcon from '@mui/icons-material/DirectionsCar';
import InfoIcon from '@mui/icons-material/Info';
import { styled } from '@mui/system';
import { useTokenDecoder, hasRole } from '../../utils/TokenUtils';
import { useAuth } from '../provider/AuthProvider';

export default function DriverInfoTab({ vehicleDetail, handleUpgrade, setOpen }) {

    const { token } = useAuth();
    const claims = useTokenDecoder(token);


    const handleClickOpen = () => {
      setOpen(true);
    };

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

    const buttonStyleLeft={
      borderRadius: {
          xs: '0 0 0 7px',
          sm: '0 0 0 10px',
          lg: '0 0 0 15px',
      },

      fontSize: {
        xs: '0.6rem',
        sm: '0.6rem',
        md: '0.9rem'
      }  
    };

    const buttonStyleRight={
      borderRadius: {
          xs: '0 0 7px 0',
          sm: '0 0 10px 0',
          lg: '0 0 15px 0',
      },

      fontSize: {
        xs: '0.6rem',
        sm: '0.6rem',
        md: '0.9rem'
      }  
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
    {hasRole(claims, "Driver") ? (
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
        <Box sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
          <Button variant="contained" color="primary" sx={buttonStyleLeft} onClick={handleClickOpen}>
              Edit driver info
          </Button>
          <Button variant="contained" color="primary" sx={buttonStyleRight} >
              Stop being a Driver
          </Button>
        </Box>
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