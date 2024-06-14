import { Box, Typography, Button, Divider } from '@mui/material';
import DirectionsCarIcon from '@mui/icons-material/DirectionsCar';
import InfoIcon from '@mui/icons-material/Info';
import { styled } from '@mui/system';
import { useTokenDecoder, hasRole } from '../../utils/TokenUtils';
import { useAuth } from '../provider/AuthProvider';
import axios from 'axios';

export default function DriverInfoTab({ vehicleDetail, setOpenEdit, setOpenUpgrade }) {

    const { token, setToken } = useAuth();
    const claims = useTokenDecoder(token);

    const handleDowngrade = () => {
      axios.put(`http://192.168.0.9:5239/user/info/downgrade/${claims.nameidentifier}`)
        .then(response => {
          setToken(response.data);
        })
        .catch((error) => {
          const { data } = error.response;
          console.log(data);
        });
    };

    const handleOpenEditDialog = () => {
      setOpenEdit(true);
    };

    const handleOpenUpgradeDialog = () => {
      setOpenUpgrade(true);
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

    const buttonInfoStyle = {
      fontSize: {
        xs: "0.8rem",
        sm: "0.8rem",
        md: "0.9rem",
      },
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
    {hasRole(claims, "Driver") && vehicleDetail ? (
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
          <Button variant="contained" color="primary" sx={buttonStyleLeft} onClick={handleOpenEditDialog}>
              Edit driver info
          </Button>
          <Button variant="contained" color="primary" sx={buttonStyleRight} onClick={handleDowngrade} >
              Stop being a Driver
          </Button>
        </Box>
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
    )}
  </Box>
  );
}