import { Box, Typography, Button, Avatar, Divider } from '@mui/material';
import { deepPurple } from '@mui/material/colors';
import { styled } from '@mui/system';

export default function UserInfoTab({userInfo, setOpen}) {

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
  
    const buttonStyle={
      borderRadius: {
          xs: '0 0 5px 5px',
          sm: '0 0 10px 10px',
          lg: '0 0 15px 15px',
          xl: '0 0 25px 25px',
      },
      mt: {
          xs: '6px',
          sm: '6px',
          lg: '6px',
          xl: '6px',
      },
      fontSize: {
        xs: '0.8rem',
        sm: '0.9rem',
        md: '1.0rem'
      }  
  };
  
    const DetailBox = styled(Box)(({ theme }) => ({
      display: 'flex',
      alignItems: 'center',
      padding: theme.spacing(1)
    }));

  return (
    <Box sx={{ mt: 3 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', mb: 3 }}>
      <Avatar src="src/assets/images/profile-img-man.png" 
              sx={{ bgcolor: deepPurple[500], width: 56, height: 56, mr: 2 }}
      >
        {userInfo.firstName[0]}
      </Avatar>
      <Typography variant="h5" sx={{ fontWeight: 'bold', ...typographyStyle }}>
        {userInfo.firstName} {userInfo.lastName}
      </Typography>
    </Box>
    <Divider sx={{ mb: 2 }} />
    <DetailBox>
      <Typography sx={{ ...typographyStyle, fontWeight: 'bold', width: '150px' }}>Gender:</Typography>
      <Typography sx={typographyStyle}>{userInfo.gender}</Typography>
    </DetailBox>
    <DetailBox>
      <Typography sx={{ ...typographyStyle, fontWeight: 'bold', width: '150px' }}>Date of Birth:</Typography>
      <Typography sx={typographyStyle}>{new Date(userInfo.dateOfBirth).toLocaleDateString()}</Typography>
    </DetailBox>
    <DetailBox>
      <Typography sx={{ ...typographyStyle, fontWeight: 'bold', width: '150px' }}>Email:</Typography>
      <Typography sx={typographyStyle}>{userInfo.email}</Typography>
    </DetailBox>
    <DetailBox>
      <Typography sx={{ ...typographyStyle, fontWeight: 'bold', width: '150px' }}>Phone Number:</Typography>
      <Typography sx={typographyStyle}>{userInfo.phoneNumber}</Typography>
    </DetailBox>
    <Button fullWidth variant="contained" color="primary" onClick={handleClickOpen} sx={{ ...buttonStyle, mt: 2 }}>
      Edit Info
    </Button>
  </Box>
  )
}