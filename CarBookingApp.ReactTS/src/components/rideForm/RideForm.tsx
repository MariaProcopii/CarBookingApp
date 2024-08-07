import * as Yup from "yup";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import { TDestination } from "../../models/Destination";
import { useState, useEffect } from "react";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { transformDateTime } from "../../utils/DateTimeUtils";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import {
    Form, Formik,
    Field, ErrorMessage,
    FormikHelpers
} from "formik";
import {
    Grid, Paper, TextField,
    Button, Typography, FormControl,
    InputLabel, Select, MenuItem,
    CssBaseline, Checkbox, Autocomplete, useTheme
} from "@mui/material";
import useAuth from "../../context/auth/UseAuth";
import { useTokenDecoder } from "../../utils/TokenUtils";

interface Props {
    rideData: any;
    handleSubmit: any;
    setSnackbar: any;
    titleText: any;
}

export default function RideForm({ rideData, handleSubmit, setSnackbar, titleText }: Props) {
    const theme = useTheme();

    const paperStyle = {
        padding: 40,
        height: "auto",
        width: "100%",
        maxWidth: 700,
        margin: "5vh auto",
        borderTop: "10px solid",
        borderRadius: "10px",
        borderColor: theme?.palette.primary.main
    };
    const buttonStyle = {
        borderRadius: {
            xs: "0 0 5px 5px",
            sm: "0 0 10px 10px",
            lg: "0 0 15px 15px",
            xl: "0 0 25px 25px",
        },
        mt: {
            xs: "6px",
            sm: "6px",
            lg: "6px",
            xl: "6px",
        },
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1.0rem",
        },
    };

    const [dateTime, setDateTime] = useState(rideData.dateOfTheRide);
    const [destinationFrom, setDestinationFrom] = useState(rideData.destinationFrom);
    const [destinationTo, setDestinationTo] = useState(rideData.destinationTo);
    const [isDateTimeValid, setDateTimeValid] = useState(true);
    const [facilities, setFacilities] = useState([]);
    const [destinations, setDestinations] = useState<TDestination[]>([] as TDestination[]);

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const {
        fetchDestinations,
        fetchFacilities,
    } = RideService(instance);

    const initialValues = {
        totalSeats: rideData.totalSeats,
        pickUpSpot: rideData.rideDetail.pickUpSpot,
        price: rideData.rideDetail.price,
        facilities: rideData.rideDetail.facilities,
    };

    const validationSchema = Yup.object().shape({
        totalSeats: Yup.number()
            .required("Total seats are required")
            .min(1, "At least one seat is required"),
        pickUpSpot: Yup.string().required("Pick up spot is required"),
        price: Yup.number()
            .required("Price is required")
            .positive("Price must be a positive number"),
    });

    useEffect(() => {
        setTimeout(() => {
            fetchDestinations(setDestinations);
            fetchFacilities(setFacilities);
        }, 10);
    }, []);

    const handleChangeDateTime = (dateTime: string) => {
        setDateTime(dateTime);
    };

    const checkDateTimeValid = () => {
        const rideDate = new Date(dateTime);
        const now = new Date();

        if (rideDate > now) {
            setDateTimeValid(true);
        } else {
            setDateTimeValid(false);
        }
    };

    const onSubmit = async (
        values: {
            totalSeats: any;
            pickUpSpot: any;
            price: any;
            facilities: any;
        }, 
        _: FormikHelpers<{
            totalSeats: any;
            pickUpSpot: any;
            price: any;
            facilities: any;
        }>
    ) => {
        checkDateTimeValid();
        if (!isDateTimeValid) {
            return;
        }

        const rideData = {
            dateOfTheRide: transformDateTime(dateTime),
            destinationFrom: destinationFrom,
            destinationTo: destinationTo,
            totalSeats: values.totalSeats,
            rideDetail: {
                pickUpSpot: values.pickUpSpot,
                price: values.price,
                facilities: values.facilities,
            },
        };

        await handleSubmit(
            claims.nameidentifier as string,
            rideData,
            setSnackbar,
        );
    };

    return (
        <>
            <CssBaseline />
            <Paper elevation={8} style={paperStyle} >
                <Grid
                    container
                    direction="column"
                    justifyContent="center"
                    alignContent="center"
                >
                    <Typography variant="h5" mb={2}>
                        {titleText} 
                    </Typography>
                </Grid>
                <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
                    {(props) => (
                        <Form>
                            <Grid container spacing={2}>
                                <Grid item xs={12} md={6}>
                                    <Autocomplete
                                        fullWidth
                                        disablePortal
                                        autoSelect
                                        autoHighlight
                                        options={destinations}
                                        value={destinationFrom}
                                        onChange={(_, newValue: TDestination) => setDestinationFrom(newValue ? newValue.label : "")}
                                        renderInput={(params) => (
                                            <TextField
                                                {...params}
                                                label="From"
                                                name="destinationFrom"
                                                placeholder="Enter destination"
                                                fullWidth
                                                required
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <Autocomplete
                                        fullWidth
                                        disablePortal
                                        autoSelect
                                        autoHighlight
                                        options={destinations}
                                        value={destinationTo}
                                        onChange={(_, newValue: TDestination) => setDestinationTo(newValue ? newValue.label : "")}
                                        renderInput={(params) => (
                                            <TextField
                                                {...params}
                                                label="To"
                                                name="destinationTo"
                                                placeholder="Enter destination"
                                                fullWidth
                                                required
                                            />
                                        )}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                                        <DemoContainer components={["DateTimePicker"]} >
                                            <Field
                                                as={DateTimePicker}
                                                label="Date of the Ride"
                                                name="dateOfTheRide"
                                                value={dateTime}
                                                onChange={handleChangeDateTime}
                                                required
                                                fullWidth
                                                sx={{ mb: 10 }}
                                            />
                                        </DemoContainer>
                                    </LocalizationProvider>
                                    {!isDateTimeValid && (
                                        <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                            Date and time must be in the future
                                        </Typography>
                                    )}
                                </Grid>
                                <Grid item xs={12} md={6} mt={1}>
                                    <Field
                                        as={TextField}
                                        label="Total Seats"
                                        name="totalSeats"
                                        type="number"
                                        placeholder="Enter seats amount"
                                        fullWidth
                                        required
                                        helperText={<ErrorMessage name="totalSeats" />}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <Field
                                        as={TextField}
                                        label="Pick Up Spot"
                                        name="pickUpSpot"
                                        placeholder="Enter pick up spot"
                                        fullWidth
                                        required
                                        helperText={<ErrorMessage name="pickUpSpot" />}
                                    />
                                </Grid>
                                <Grid item xs={12} md={6}>
                                    <Field
                                        as={TextField}
                                        label="Price"
                                        name="price"
                                        type="number"
                                        placeholder="Currency in lei "
                                        fullWidth
                                        required
                                        helperText={<ErrorMessage name="price" />}
                                    />
                                </Grid>
                                <Grid item xs={12}>
                                    <FormControl fullWidth>
                                        <InputLabel>Facilities</InputLabel>
                                        <Field
                                            as={Select}
                                            name="facilities"
                                            label="Facilities"
                                            multiple
                                            renderValue={(selected: Array<any>) => selected.join(", ")}
                                        >
                                            {facilities.map((facility) => (
                                                <MenuItem key={facility} value={facility}>
                                                    <Checkbox checked={props.values.facilities.includes(facility)} />
                                                    {facility}
                                                </MenuItem>
                                            ))}
                                        </Field>
                                    </FormControl>
                                </Grid>
                                <Grid item xs={12}>
                                    <Button
                                        type="submit"
                                        color="primary"
                                        variant="contained"
                                        fullWidth
                                        sx={buttonStyle}
                                    >
                                        {titleText}
                                    </Button>
                                </Grid>
                            </Grid>
                        </Form>
                    )}
                </Formik>
            </Paper>
        </>
    );
}