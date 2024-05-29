import axios from 'axios';
import { useState } from 'react';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import FormControl from '@mui/material/FormControl';
import InputLabel from '@mui/material/InputLabel';
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/provider/AuthProvider";


export default function SignUp({setUserLoggedIn}) {
    const [gender, setGender] = useState('');
    const [date, setDate] = useState(null);
    const [email, setEmail] = useState('');
    const [isEmailValid, setEmailValid] = useState(false);
    const [phone, setPhone] = useState('');
    const [isPhoneValid, setPhoneValid] = useState(false);

    const { setToken } = useAuth();
    const navigate = useNavigate();

    function transformDate(inputDate) {
      const [month, day, year] = inputDate.split('/');
  
      if (month && day && year) {
          return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
      } else {
          return null;
      }
    }

    const handleChangeGender = (event) => {
        setGender(event.target.value);
    };

    const handleChangeDate = (date) => {
        setDate(date);
    };

    const handleChangeEmail = (event) => {
        const email = event.target.value;
        setEmail(email);

        const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

        if (re.test(email)) {
            setEmailValid(true);
        }
        else {
            setEmailValid(false);
        }
    };

    const handleChangePhone = (event) => {
        const phone = event.target.value;
        setPhone(phone);

        const re = /^[0-9]{9}$/;

        if (re.test(phone)) {
            setPhoneValid(true);
        }
        else {
            setPhoneValid(false);
        }
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);

        if (!isEmailValid) {
            console.log("Wrong email");
            return;
        }

        if (!isPhoneValid) {
            console.log("Wrong phone");
            return;
        }

        if (data.get('password') != data.get('conf_password')) {
            console.log("Wrong password");
            return;
        }

        axios.post(
          "http://localhost:5239/Auth/SignUp",
          {
            firstName: data.get('firstname'),
            lastName: data.get('lastname'), 
            dateOfBirth: date ? transformDate(new Date(date).toLocaleString().split(",")[0]) : "",
            gender: gender.toUpperCase(),
            email: email,
            phoneNumber: phone,
            password: data.get('password'),
          },
        )
        .then((response) => {
          setToken(response.data);
          navigate("/", { replace: true});
        })
        .catch((error) => {
          console.log(error);
        });
    };


    return (
        <Container 
          component="main" 
          maxWidth="xs"
        >
          <CssBaseline />
          <Box
            sx={{
              marginTop: 8,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          >
            <Avatar sx={{ m: 1, bgcolor: 'primary.main' }}>
              <LockOutlinedIcon />
            </Avatar>
            <Typography component="h1" variant="h5">
              Sign Up 
            </Typography>
            <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
              <TextField
                margin="normal"
                required
                fullWidth
                id="firstname"
                label="Firstname"
                name="firstname"
              />
              <TextField
                margin="normal"
                required
                fullWidth
                id="lastname"
                label="Lastname"
                name="lastname"
              />
              <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <DemoContainer components={['DatePicker']}>
                      <DatePicker 
                          label="Pick your birthday" 
                          value={date}
                          onChange={handleChangeDate}
                          sx={{
                            width: '400px',
                          }}
                      />
                  </DemoContainer>
              </LocalizationProvider>
              <FormControl 
                margin="normal"
                fullWidth
              >
                  <InputLabel id="gender-select-label">Gender</InputLabel>
                  <Select
                      labelId="gender-select-label"
                      id="gender"
                      value={gender}
                      label="Gender"
                      onChange={handleChangeGender}
                  >
                      <MenuItem value={"Female"}>Female</MenuItem>
                      <MenuItem value={"Male"}>Male</MenuItem>
                  </Select>
              </FormControl>
              <TextField
                margin="normal"
                required
                fullWidth
                id="email"
                label="Email Address"
                name="email"
                autoComplete="email"
                value={email}
                onChange={handleChangeEmail}
              />
              <TextField
                margin="normal"
                required
                fullWidth
                id="phone"
                label="Phone Number"
                name="phone"
                autoComplete="phone"
                value={phone}
                onChange={handleChangePhone}
              />
              <TextField
                margin="normal"
                required
                fullWidth
                name="password"
                label="Password"
                type="password"
                id="password"
                autoComplete="current-password"
              />
              <TextField
                margin="normal"
                required
                fullWidth
                name="conf_password"
                label="Confirm Password"
                type="password"
                id="conf_password"
                autoComplete="current-password"
              />
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
              >
                Sign Up
              </Button>
            </Box>
          </Box>
        </Container>
      );
}
  