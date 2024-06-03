import { Grid, TextField, Button, CircularProgress, Box, Paper, Autocomplete } from '@mui/material';
import { useState, useEffect } from 'react';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
// import parse from 'autosuggest-highlight/parse';
// import match from 'autosuggest-highlight/match';
import axios from 'axios';

export default function SearchBar() {
  const [destinationFrom, setDestinationFrom] = useState('');
  const [destinationTo, setDestinationTo] = useState('');
  const [date, setDate] = useState(null);
  const [isDateValid, setDateValid] = useState(true);
  const [seats, setSeats] = useState(1);
  const [loading, setLoading] = useState(false);
  const [destinations, setDestinations] = useState([]);

  useEffect(() => {
    fetchDestinations();
  }, []);

  const fetchDestinations = () => {
    axios.get("http://localhost:5239/destination/pick/name")
      .then((response) => {
        setDestinations(response.data.items);
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

const top100Films = [
    { label: 'The Shawshank Redemption', year: 1994 },
    { label: 'The Godfather', year: 1972 },
    { label: 'The Godfather: Part II', year: 1974 },
    { label: 'The Dark Knight', year: 2008 },
    { label: '12 Angry Men', year: 1957 },
    { label: "Schindler's List", year: 1993 },
    { label: 'Pulp Fiction', year: 1994 },];

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
      <Paper elevation={8} sx={{ width: { xs: '100%', sm: '90%', md: '90%' }, padding: 1}}>
        <Grid container direction="row" spacing={2} justifyContent="center" alignItems="center">
          {/* <Grid item xs={12} sm={6} md={4} lg={2}>
            <TextField
              fullWidth
              size="small"
              inputProps={{ maxLength: 30 }}
              label="From"
              value={destinationFrom}
              onChange={(e) => setDestinationFrom(e.target.value)}
              InputLabelProps= {{
                sx: inputBodyTextStyle
              }}
              InputProps= {{ sx: inputBodyTextStyle }}
            />
          </Grid> */}
          {/* <Grid item xs={12} sm={6} md={4} lg={2}>
            <TextField
              fullWidth
              size="small"
              inputProps={{ maxLength: 30 }}
              label="To"
              value={destinationTo}
              onChange={(e) => setDestinationTo(e.target.value)}
              InputLabelProps= {{
                sx: inputBodyTextStyle
              }}
              InputProps= {{ sx: inputBodyTextStyle }}
            />
          </Grid> */}
            <Grid item xs={12} sm={6} md={4} lg={2}>
          <Autocomplete
            disablePortal
            fullWidth
            size="small"
            inputProps={{ minLength: 30 }}
            label="From"
            sx={{ minWidth:'100px' }}
            options={top100Films}
            renderInput={(params) => <TextField {...params} label="Movie" />}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2}>
          <Autocomplete
            disablePortal
            fullWidth
            size="small"
            inputProps={{ minLength: 30 }}
            label="To"
            sx={{ minWidth:'100px' }}
            options={top100Films}
            renderInput={(params) => <TextField {...params} label="Movie" />}
            />
          </Grid>
            {/* <Grid item xs={12} sm={6} md={4} lg={2}>
            <Autocomplete
              fullWidth
              size="small"
              options={destinations}
              getOptionLabel={(option) => option.name}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="From"
                  InputLabelProps={{
                    sx: inputBodyTextStyle
                  }}
                  inputProps={{
                    ...params.inputProps,
                    maxLength: 30
                  }}
                  fullWidth
                  size="small"
                />
              )}
              onChange={(event, newValue) => {
                setDestinationFrom(newValue ? newValue.name : '');
              }}
              renderOption={(props, option, { inputValue }) => {
                const matches = match(option.name, inputValue, { insideWords: true });
                const parts = parse(option.name, matches);

                return (
                  <li {...props}>
                    <div>
                      {parts.map((part, index) => (
                        <span
                          key={index}
                          style={{
                            fontWeight: part.highlight ? 700 : 400,
                          }}
                        >
                          {part.text}
                        </span>
                      ))}
                    </div>
                  </li>
                );
              }}
            />
          </Grid>
          <Grid item xs={12} sm={6} md={4} lg={2}>
            <Autocomplete
            //   sx={{ width: 300 }}
              options={destinations}
              getOptionLabel={(option) => option.name}
              renderInput={(params) => (
                <TextField
                  {...params}
                  label="To"
                  InputLabelProps={{
                    sx: inputBodyTextStyle
                  }}
                  inputProps={{
                    ...params.inputProps,
                    maxLength: 30
                  }}
                  fullWidth
                  size="small"
                />
              )}
              onChange={(event, newValue) => {
                setDestinationTo(newValue ? newValue.name : '');
              }}
              renderOption={(props, option, { inputValue }) => {
                const matches = match(option.name, inputValue, { insideWords: true });
                const parts = parse(option.name, matches);

                return (
                  <li {...props}>
                    <div>
                      {parts.map((part, index) => (
                        <span
                          key={index}
                          style={{
                            fontWeight: part.highlight ? 700 : 400,
                          }}
                        >
                          {part.text}
                        </span>
                      ))}
                    </div>
                  </li>
                );
              }}
            />
          </Grid> */}
          <Grid item xs={12} sm={6} md={4} lg={3} mt={-1} >
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DemoContainer components={['DatePicker']} sx={{ minWidth: '100px' }}>
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
          <Grid item xs={12} sm={6} md={4} lg={2}>
            <TextField
              fullWidth
              sx={{ minWidth:'100px' }}
              size="small"
              inputProps={{ maxLength: 30 }}
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
