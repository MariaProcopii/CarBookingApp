import { Box, Avatar, Typography, Button, Divider } from "@mui/material";
import { getDateFromISO, getTimeFromISO} from "../../utils/DateTimeUtils";
import React, { useEffect, useState } from 'react';
import RideDetails from "../../pages/rideDetails/RideDetails";
import getAvatarSrc from "../../utils/AvatarUtils";

export default function Ride({ride}) {
    const [openRideDetails, setOpenRideDetails] = useState(false);

    const mainBoxStyle={
        boxShadow: 3,
        borderRadius: {
            xs: '5px',
            sm: '10px',
            md: '15px',
            lg: '20px',
        },
        width: {
            xs: '100px',
            sm: '130px',
            md: '180px',
            lg: '200px',
        },
        height: {
            xs: '160px',
            sm: '200px',
            md: '260px',
            lg: '290px',
        },
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
            xs: '0.5rem',
            sm: '0.6rem',
            md: '0.8rem',
            lg: '0.9rem'
          }  
    };
    const avatarStyle={
        width: '50%',
        height: 'auto',
    };
    const headTypographyStyle={
        fontSize: {
          xs: '0.6rem',
          sm: '0.8rem',
          md: '1.0rem',
          lg: '1.1rem'
        }
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
        justifyContent: 'space-between',
        paddingLeft: '6%',
        paddingRight: '6%'
        };
        
    const InfoItem = ({ label, value }) => (
    <Box sx={spacingStyle}>
        <Typography sx={bodyTypographyStyle} variant="body1" component="span">{label}:</Typography>
        <Typography sx={bodyTypographyStyle} variant="body1" component="span">{value}</Typography>
    </Box>
    );

  return (
    <Box sx={mainBoxStyle}>
            <Box align='center'>
                <Avatar 
                    src={getAvatarSrc(ride.ownerGender)}
                    sx={avatarStyle}
                />
                <Typography sx={headTypographyStyle} variant="body1">{ride.ownerName}</Typography>
            </Box>
            <Divider />
            <Box>
                <InfoItem label="From" value={ride.destinationFrom} />
                <InfoItem label="To" value={ride.destinationTo} />
                <InfoItem label="Date" value={getDateFromISO(ride.dateOfTheRide)} />
                <InfoItem label="Time" value={getTimeFromISO(ride.dateOfTheRide)} />
                <InfoItem label="Price" value={ride.price + " lei"}/>
                <InfoItem label="Seats" value={ride.totalSeats} />
                <Button fullWidth 
                        variant="contained" 
                        sx={buttonStyle}
                        onClick={()=> setOpenRideDetails(true)}
                >
                    View Details
                </Button>
                <RideDetails openRideDetails={openRideDetails} setOpenRideDetails={setOpenRideDetails} rideId={ride.id}/>
            </Box>
    </Box>
  )
}
