import { Grid, TextField, Button, CircularProgress, Box, Paper, Autocomplete } from '@mui/material';
import { useState, useEffect } from 'react';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import axios from 'axios';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from "yup";

export default function SearchBar() {
  const [date, setDate] = useState(null);
  const [isDateValid, setDateValid] = useState(true);
  const [seats, setSeats] = useState(1);
  const [loading, setLoading] = useState(false);
  const [destinations, setDestinations] = useState([]);

  const initialValues = {
    destinationFrom: "",
    destinationTo: "",
    totalSeats: 1
};

  
  const validationSchema = Yup.object().shape({
    destinationFrom: Yup.string()
        .required("Destination From is required"),
    destinationTo: Yup.string()
        .required("Destination To is required"),
    totalSeats: Yup.number()
        .required("Number of Seats is required")
        .min(1, "Number of Seats must be greater than 0")
  });

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
        console.log(response.data);
        console.log(destinations[0]);
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
    console.log(transformDate());

  const handleSearch = () => {
      setLoading(true);
      checkDateValid();
      if (!isDateValid) {
          return;
      }
      setLoading(false);
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
            disablePortal
            sx={{ minWidth:'100px' }}
            options={destinations}
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
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2.2}>
          <Autocomplete
            disablePortal
            options={destinations}
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
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2.4} mt={-1} >
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DemoContainer components={['DatePicker']} sx={{ minWidth:'220px' }} >
                <DatePicker
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
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2.2} >
            <TextField
              fullWidth
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
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2}>
            <Button
              fullWidth
              size="small"
              variant="contained"
              color="primary"
              onClick={handleSearch}
              disabled={loading}
              sx={buttonStyle}
            >
              {loading ? <CircularProgress size={24} /> : "Search"}
            </Button>
          </Grid>
        </Grid>
      </Paper>
    </Box>
  )
}
