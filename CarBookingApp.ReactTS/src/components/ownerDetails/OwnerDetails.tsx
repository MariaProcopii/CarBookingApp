import EmailIcon from "@mui/icons-material/Email";
import PhoneIcon from "@mui/icons-material/Phone";
import InfoIcon from "@mui/icons-material/Info";
import getAvatarSrc from "../../utils/AvatarUtils";
import DirectionsCarIcon from "@mui/icons-material/DirectionsCar";
import { TOwner } from "../../models/Owner";
import { styled } from "@mui/system";
import { calculateAge } from "../../utils/DateTimeUtils";
import {
    Typography, Box, Paper,
    Grid, Avatar, Divider
} from "@mui/material";

interface Props {
    owner: TOwner;
}

export default function OwnerDetails({ owner }: Props) {
    const typographyStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
            lg: "1.1rem",
        }
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
            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }} variant="h6" gutterBottom>Owner Details</Typography>
            <Box sx={{ p: 2 }}>
                <Grid container spacing={2} alignItems="center">
                    <Grid item>
                        <Avatar
                            sx={{ bgcolor: "secondary.main", width: 56, height: 56 }}
                            src={getAvatarSrc(owner.gender)}
                        >
                            {owner.firstName.charAt(0)}{owner.lastName.charAt(0)}
                        </Avatar>
                    </Grid>
                    <Grid item xs>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Name:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{owner.firstName} {owner.lastName}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Age:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{calculateAge(owner.dateOfBirth)}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <Typography sx={{ ...typographyStyle, fontWeight: "bold" }}>Experience:</Typography>
                            <Typography sx={typographyStyle}>&nbsp;{owner.yearsOfExperience} years</Typography>
                        </DetailBox>
                        <Divider sx={{ my: 2 }} />
                        <DetailBox>
                            <EmailIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>{owner.email}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <PhoneIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>{owner.phoneNumber}</Typography>
                        </DetailBox>
                        <DetailBox>
                            <DirectionsCarIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>
                                {owner.vehicleDetail.vehicle.vender} {owner.vehicleDetail.vehicle.model} ({owner.vehicleDetail.manufactureYear})
                            </Typography>
                        </DetailBox>
                        <DetailBox>
                            <InfoIcon color="primary" sx={iconStyle} />
                            <Typography sx={typographyStyle}>{owner.vehicleDetail.registrationNumber}</Typography>
                        </DetailBox>
                    </Grid>
                </Grid>
            </Box>
        </Paper>
    );
}