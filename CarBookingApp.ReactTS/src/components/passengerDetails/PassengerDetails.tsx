import Carousel from "react-material-ui-carousel";
import EmailIcon from "@mui/icons-material/Email";
import PhoneIcon from "@mui/icons-material/Phone";
import getAvatarSrc from "../../utils/AvatarUtils";
import { TPassanger } from "../../models/Passanger";
import { styled } from "@mui/system";
import { calculateAge } from "../../utils/DateTimeUtils";
import { Typography, Box, Paper, Grid, Avatar, Divider } from "@mui/material";

interface Props {
    passengers: TPassanger[];
}

export default function PassengerDetails({ passengers }: Props) {
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
            lg: "1.1rem",
        },
    };
    const DetailBox = styled(Box)(({ theme }) => ({
        display: "flex",
        alignItems: "center",
        padding: theme.spacing(1),
    }));
    const iconStyle = {
        fontSize: {
            xs: "1rem",
            sm: "1.2rem",
            md: "1.5rem",
            lg: "1.7rem",
        },
        marginRight: "8px",
    };

    return (
        <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }} variant="h6" gutterBottom>Passengers</Typography>
            <Carousel
                animation="slide"
                interval={4000}
            >
                {passengers.map((passenger: TPassanger) => (
                    <Box key={passenger.id} sx={{ p: 2 }}>
                        <Grid container spacing={2} alignItems="center">
                            <Grid item>
                                <Avatar
                                    sx={{ bgcolor: "primary.main", width: 56, height: 56 }}
                                    src={getAvatarSrc(passenger.gender)}
                                >
                                    {passenger.firstName.charAt(0)}{passenger.lastName.charAt(0)}
                                </Avatar>
                            </Grid>
                            <Grid item xs>
                                <DetailBox>
                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Name:</Typography>
                                    <Typography sx={typographyStyle}>&nbsp;{passenger.firstName} {passenger.lastName}</Typography>
                                </DetailBox>
                                <DetailBox>
                                    <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Age:</Typography>
                                    <Typography sx={typographyStyle}>&nbsp;{calculateAge(passenger.dateOfBirth)}</Typography>
                                </DetailBox>
                                <Divider sx={{ my: 2 }} />
                                <DetailBox>
                                    <EmailIcon color="primary" sx={iconStyle} />
                                    <Typography sx={typographyStyle}>{passenger.email}</Typography>
                                </DetailBox>
                                <DetailBox>
                                    <PhoneIcon color="primary" sx={iconStyle} />
                                    <Typography sx={typographyStyle}>{passenger.phoneNumber}</Typography>
                                </DetailBox>
                            </Grid>
                        </Grid>
                    </Box>
                ))}
            </Carousel>
        </Paper>
    );
}