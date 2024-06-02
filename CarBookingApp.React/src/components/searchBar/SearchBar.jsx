import { Grid, TextField, Button, CircularProgress, Box, Paper } from '@mui/material';
import { useState } from 'react';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';

export default function SearchBar() {
  const [destinationFrom, setDestinationFrom] = useState('');
  const [destinationTo, setDestinationTo] = useState('');
  const [date, setDate] = useState(null);
  const [isDateValid, setDateValid] = useState(true);
  const [seats, setSeats] = useState(1);
  const [loading, setLoading] = useState(false);
  const paperStyle={height:'10vh',width:'90%', margin:'20px'};

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
      <Paper elevation={8} sx={{ width: { xs: '100%', sm: '90%', md: '80%' }, padding: 1 }}>
        <Grid container direction="row" spacing={2} justifyContent="center" alignItems="center">
          <Grid item xs={12} sm={6} md={2}>
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
          </Grid>
          <Grid item xs={12} sm={6} md={2}>
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
          </Grid>
          <Grid item xs={12} sm={6} md={2.9} mt={-1}>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DemoContainer components={['DatePicker']}>
                <DatePicker
                  label="Date"
                  value={date}
                  onChange={handleChangeDate}
                  slotProps={
                    { 
                    textField: 
                    {
                        // style: {overflow: 'auto hidden'},
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
          <Grid item xs={12} sm={6} md={2}>
            <TextField
              fullWidth
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
          <Grid item xs={12} sm={6} md={2}>
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
