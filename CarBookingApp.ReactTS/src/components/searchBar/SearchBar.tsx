import dayjs from "dayjs";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import { TRide } from "../../models/Ride";
import { TDestination } from "../../models/Destination";
import { useState, useEffect } from "react";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import {
    Grid, TextField, Button,
    Box, Paper, Autocomplete, Typography
} from "@mui/material";
import useAuth from "../../context/auth/UseAuth";
import { useTokenDecoder } from "../../utils/TokenUtils";

interface Props {
    setRides: (newRides: TRide[]) => void;
    setTotalPages: (newTotalPages: number) => void;
    handleClose: () => void;
}

export default function SearchBar({ setRides, setTotalPages, handleClose }: Props) {
    const boxStyle = {
        display: "flex",
        justifyContent: "center",
        mt: 1,
    };
    const paperStyle = {
        width: {
            xs: "100%"
        },
        padding: 1,
    };
    const buttonStyle = {
        fontSize: {
            xs: "0.5rem",
            sm: "0.6rem",
            md: "0.8rem",
            lg: "0.9rem",
        },
    };
    const inputBodyTextStyle = {
        fontSize: {
            xs: "0.8rem",
            sm: "0.9rem",
            md: "1rem",
            lg: "1.1rem",
        },
    };

    const [date, setDate] = useState(dayjs().format("YYYY-MM-DD"));
    const [isDateValid, setDateValid] = useState(true);
    const [seats, setSeats] = useState(1);
    const [destinations, setDestinations] = useState<TDestination[]>([] as TDestination[]);
    const [destinationFrom, setDestinationFrom] = useState("");
    const [destinationTo, setDestinationTo] = useState("");
    const [showErrorMessage, setShowErrorMessage] = useState(false);

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const {
        fetchRidesWithParams,
        fetchDestinations,
    } = RideService(instance);

    const checkDateValid = () => {
        const re = /^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/;

        if (re.test(date)) {
            setDateValid(true);
        }
        else {
            setDateValid(false);
        }
    };

    const validateInputs = () => {
        checkDateValid();
        setShowErrorMessage(true);
        if (!isDateValid) {
            return;
        }
        if (!date || !isDateValid || !destinationFrom ||
            !destinationTo || seats <= 0 || !seats) {
            return false;
        }
        setShowErrorMessage(false);
        return true;
    };

    const handleSearch = () => {
        if (!validateInputs()) {
            return;
        }
        fetchRidesWithParams(
            claims.nameidentifier as string,
            {
                date,
                destinationFrom,
                destinationTo,
                seats,
            },
            setRides,
            setTotalPages,
            handleClose,
        );
    };

    useEffect(() => {
        setTimeout(() => {
            fetchDestinations(setDestinations);
        }, 1000);
    }, []);

    return (
        <Box sx={boxStyle}>
            <Paper elevation={8} sx={paperStyle}>
                <Grid container direction="row" spacing={2} justifyContent="center" alignItems="center">
                    <Grid item xs={12} sm={6} md={4} lg={2.2}>
                        <Autocomplete
                            aria-required
                            disablePortal
                            autoSelect
                            autoHighlight
                            sx={{ minWidth: "100px" }}
                            options={destinations}
                            onChange={(_, newValue: TDestination) => setDestinationFrom(newValue ? newValue.label : "")}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    sx={{ minWidth: "200px" }}
                                    label="From"
                                    InputLabelProps={{
                                        sx: inputBodyTextStyle,
                                    }}
                                    inputProps={{
                                        ...params.inputProps,
                                        maxLength: 30,
                                        sx: inputBodyTextStyle,
                                    }}
                                    fullWidth
                                    required
                                    size="small"
                                />
                            )}
                        />
                        {showErrorMessage && !destinationFrom && (
                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                Destination From required
                            </Typography>
                        )}
                    </Grid>
                    <Grid item xs={12} sm={6} md={4} lg={2.2}>
                        <Autocomplete
                            aria-required
                            disablePortal
                            autoSelect
                            autoHighlight
                            options={destinations}
                            onChange={(_, newValue: TDestination) => setDestinationTo(newValue ? newValue.label : "")}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    sx={{ minWidth: "200px" }}
                                    label="To"
                                    InputLabelProps={{
                                        sx: inputBodyTextStyle,
                                    }}
                                    inputProps={{
                                        ...params.inputProps,
                                        maxLength: 30,
                                        sx: inputBodyTextStyle,
                                    }}
                                    fullWidth
                                    required
                                    size="small"
                                />
                            )}
                        />
                        {showErrorMessage && !destinationTo && (
                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                Destination To is required
                            </Typography>
                        )}
                    </Grid>
                    <Grid item xs={12} sm={6} md={4} lg={2.4} mt={-1} >
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DemoContainer components={["DatePicker"]} sx={{ minWidth: "220px" }} >
                                <DatePicker
                                    openTo="month"
                                    views={["year", "month", "day"]}
                                    label="Date"
                                    value={dayjs(date)}
                                    onChange={(datejs) => setDate(datejs ? datejs.format("YYYY-MM-DD") : "")}
                                    slotProps={
                                        {
                                            textField:
                                            {
                                                // style: {overflow: "hidden"},
                                                size: "small",
                                                inputProps: { maxLength: 30 },
                                                InputLabelProps: { sx: inputBodyTextStyle },
                                                InputProps: { sx: inputBodyTextStyle }
                                            }
                                        }}
                                />
                            </DemoContainer>
                        </LocalizationProvider>
                        {!date && (
                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                Date is required
                            </Typography>
                        )}
                    </Grid>
                    <Grid item xs={12} sm={6} md={4} lg={2.2} >
                        <TextField
                            fullWidth
                            required
                            sx={{ minWidth: "200px" }}
                            size="small"
                            inputProps={{ maxLength: 30 }}
                            label="Seats"
                            type="number"
                            value={seats}
                            onChange={(e) => setSeats(parseInt(e.target.value, 10))}
                            InputLabelProps={{
                                sx: inputBodyTextStyle
                            }}
                            InputProps={{ sx: inputBodyTextStyle }}
                        />
                        {seats <= 0 && (
                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                Number must be greater than 0
                            </Typography>
                        )}
                    </Grid>
                    <Grid item xs={12} sm={6} md={4} lg={2}>
                        <Button
                            fullWidth
                            size="small"
                            variant="contained"
                            color="primary"
                            onClick={handleSearch}
                            sx={buttonStyle}
                        >
                            Search
                        </Button>
                    </Grid>
                </Grid>
            </Paper>
        </Box>
    )
}