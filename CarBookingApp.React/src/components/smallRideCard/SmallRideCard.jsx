import { Box, Avatar, Typography, Button, Divider, CardMedia, Card, CardContent, CardActions } from "@mui/material";
import { getDateFromISO, getTimeFromISO} from "../../utils/DateTimeUtils";
import React, { useEffect, useState } from 'react';
import { useTheme } from '@mui/material/styles';
import RideDetails from "../rideDetails/RideDetails";

export default function SmallRideCard({ride, edit}) {
    const [openRideDetails, setOpenRideDetails] = useState(false);
    const theme = useTheme();

    const mainBoxStyle={
        boxShadow: 3,
        borderRadius: {
            xs: '5px',
            sm: '10px',
            md: '15px',
            lg: '20px',
        },
        width: {
            xs: '125px',
            sm: '160px',
            md: '180px',
            lg: '200px',
        },
        height: {
            xs: '196px',
            sm: '216px',
            md: '260px',
            lg: '280px',
        },
    };

    const buttonStyle={
        borderRadius: {
            xs: '0 0 5px 5px',
            sm: '0 0 10px 10px',
            lg: '0 0 15px 15px',
            xl: '0 0 25px 25px',
        },
        fontSize: {
            xs: '0.5rem',
            sm: '0.6rem',
            md: '0.8rem',
            lg: '0.9rem'
          }  
    };

    const imageStyle={
        width: '100%',
        height: '40%',
    };

    const bodyTypographyStyle={
        fontSize: {
            xs: '0.5rem',
            sm: '0.6rem',
            md: '0.8rem',
            lg: '0.9rem'
          }
    };

    const spacingStyle = {
        display: 'flex',
        justifyContent: 'space-between'
    };

    const getImagePath = () => {
        if (theme.palette.mode === 'dark') {
            return "src/assets/images/blue-dark-small-car.png";
        } else {
            return "src/assets/images/blue-light-small-car.png";
        }
    };
        
    const InfoItem = ({ label, value }) => (
    <Box sx={spacingStyle}>
        <Typography sx={bodyTypographyStyle} variant="body1" component="span">{label}:</Typography>
        <Typography sx={bodyTypographyStyle} variant="body1" component="span">{value}</Typography>
    </Box>
    );

  return (
        <Card sx={mainBoxStyle}>
            <CardMedia
                sx={imageStyle}
                image={getImagePath()}
            />
        <CardContent>
            <Box>
                <InfoItem label="From" value={ride.destinationFrom} />
                <InfoItem label="To" value={ride.destinationTo} />
                <InfoItem label="Date" value={getDateFromISO(ride.dateOfTheRide)} />
                <InfoItem label="Time" value={getTimeFromISO(ride.dateOfTheRide)} />
            </Box>
        </CardContent>
        <Box mb={1.5} />
        <Button fullWidth 
                variant="contained" 
                sx={buttonStyle}
                onClick={()=> setOpenRideDetails(true)}
        >
            View Details
        </Button>
        <RideDetails 
            openRideDetails={openRideDetails} 
            setOpenRideDetails={setOpenRideDetails}
            rideId={ride.id}
            edit={edit}
        />
  </Card>
  )
}