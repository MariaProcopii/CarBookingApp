import { Grid, TextField, Button, CircularProgress, Box, Paper, Autocomplete, Typography } from '@mui/material';
import { useState, useEffect } from 'react';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import axios from 'axios';
import dayjs from 'dayjs';

export default function SearchBar({ onSearch }) {
  const [date, setDate] = useState(dayjs());
  const [isDateValid, setDateValid] = useState(true);
  const [seats, setSeats] = useState(1);
  const [destinations, setDestinations] = useState([]);
  const [destinationFrom, setDestinationFrom] = useState('Chisinau');
  const [destinationTo, setDestinationTo] = useState('Soroca');

  useEffect(() => {
    setTimeout(() => {
        fetchDestinations();
      }, 1000);
  }, []);

  const fetchDestinations = () => {
    axios.get("http://localhost:5239/destination/pick/name")
      .then((response) => {
        const topCities = response.data.map(city => ({ label: city}));
        setDestinations(topCities);
      })
      .catch((error) => {
        console.error('Error fetching destinations:', error);
      });
  };

  const handleChangeDate = (date) => {
    setDate(date);
  };

  const checkDateValid = () => {
    const re = /^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/;
    const birthday = transformDate();

    if (re.test(birthday)) {
        setDateValid(true);
    }
    else {
        setDateValid(false);
    }
};

    function transformDate() {
        const formatedDate = new Date(date).toLocaleString().split(",")[0];
        const [month, day, year] = formatedDate.split('/');

        if (month && day && year) {
            return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
        } else {
            return null;
        }
    };

const validateInputs = () => {
    checkDateValid();
    if (!isDateValid) {
        return;
    }
    if (!date || !isDateValid || !destinationFrom || !destinationTo || seats <= 0 || !seats) {
      return false;
    }
    return true;
  };

  const searchParams = {
    date: transformDate(),
    destinationFrom,
    destinationTo,
    seats,
  };

  const handleSearch = () => {
    if (!validateInputs()) {
      return;
    }
    onSearch(searchParams);
};

  const buttonStyle={
    fontSize: {
        xs: '0.5rem',
        sm: '0.6rem',
        md: '0.8rem',
        lg: '0.9rem'
      }  
  };
  const inputBodyTextStyle = {
    fontSize: {
      xs: '0.8rem',
      sm: '0.9rem',
      md: '1rem',
      lg: '1.1rem',
    },
  };
      
  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 1 }}>
      <Paper elevation={8} sx={{ width: { xs: '100%' }, padding: 1}}>
            <Grid container direction="row" spacing={2} justifyContent="center" alignItems="center">
                <Grid item xs={12} sm={6} md={4} lg={2.2}>
                    <Autocomplete
                        required
                        disablePortal
                        sx={{ minWidth:'100px' }}
                        options={destinations}
                        onChange={(event, newValue) => setDestinationFrom(newValue ? newValue.label : '')}
                        renderInput={(params) => (
                            <TextField
                            {...params}
                            sx={{ minWidth:'200px' }}
                            label="From"
                            InputLabelProps={{
                                sx: inputBodyTextStyle
                            }}
                            inputProps={{
                                ...params.inputProps,
                                maxLength: 30,
                                sx: inputBodyTextStyle
                            }}
                            fullWidth
                            size="small"
                            />
                        )}
                        />
                        {!destinationFrom && (
                        <Typography color="gray" variant="caption" sx={{ml:2}}>
                            Destination From required
                        </Typography>
                        )}
                    </Grid>
                <Grid item xs={12} sm={6} md={4} lg={2.2}>
                <Autocomplete
                    required
                    disablePortal
                    options={destinations}
                    onChange={(event, newValue) => setDestinationTo(newValue ? newValue.label : '')}
                    renderInput={(params) => (
                        <TextField
                        {...params}
                        sx={{ minWidth:'200px' }}
                        label="To"
                        InputLabelProps={{
                            sx: inputBodyTextStyle
                        }}
                        inputProps={{
                            ...params.inputProps,
                            maxLength: 30,
                            sx: inputBodyTextStyle
                        }}
                        fullWidth
                        size="small"
                        />
                    )}
                    />
                    {!destinationTo && (
                        <Typography color="gray" variant="caption" sx={{ml:2}}>
                            Destination To is required
                        </Typography>
                    )}
                </Grid>
                <Grid item xs={12} sm={6} md={4} lg={2.4} mt={-1} >
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DemoContainer components={['DatePicker']} sx={{ minWidth:'220px' }} >
                            <DatePicker
                            openTo="month"
                            views={['year', 'month', 'day']}
                            label="Date"
                            value={date}
                            onChange={handleChangeDate}
                            slotProps={
                                { 
                                textField: 
                                {
                                    // style: {overflow: 'hidden'},
                                    size: 'small',
                                    inputProps: { maxLength: 30 }, 
                                    InputLabelProps: { sx: inputBodyTextStyle }, 
                                    InputProps: { sx: inputBodyTextStyle }
                                }
                            }}
                            />
                        </DemoContainer>
                    </LocalizationProvider>
                    {!date && (
                        <Typography color="gray" variant="caption" sx={{ml:2}}>
                            Date is required
                        </Typography>
                    )}
                </Grid>
                <Grid item xs={12} sm={6} md={4} lg={2.2} >
                    <TextField
                    fullWidth
                    required
                    sx={{ minWidth:'200px' }}
                    size="small"
                    inputProps={{ maxLength: 30}}
                    label="Seats"
                    type="number"
                    value={seats}
                    onChange={(e) => setSeats(e.target.value)}
                    InputLabelProps= {{
                        sx: inputBodyTextStyle
                    }}
                    InputProps= {{ sx: inputBodyTextStyle }}
                    />
                    {seats <= 0 && (
                        <Typography color="gray" variant="caption" sx={{ml:2}}>
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
