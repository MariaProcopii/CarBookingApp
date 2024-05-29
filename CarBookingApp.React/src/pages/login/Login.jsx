import Avatar from '@mui/material/Avatar'; 
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import {useState} from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/provider/AuthProvider";
import axios from 'axios';


export default function Login() {
    const [email, setEmail] = useState('');
    const [isEmailValid, setEmailValid] = useState(false);

    const { setToken } = useAuth();
    const navigate = useNavigate();

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

    const handleSubmit = (event) => {
        event.preventDefault();
        const data = new FormData(event.currentTarget);

        if (!isEmailValid) {
            console.log("Wrong email");
            return ;
        }

        axios.post(
          "http://localhost:5239/Auth/LogIn",
          {
            email: data.get('email'),
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
            marginTop: 10,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
          }}
        >
          <Avatar sx={{ m: 1, bgcolor: 'primary.main' }}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            Log in
          </Typography>
          <Box component="form" onSubmit={handleSubmit} noValidate sx={{ mt: 1 }}>
            <TextField
              margin="normal"
              required
              fullWidth
              id="email"
              label="Email Address"
              name="email"
              autoComplete="email"
              autoFocus
              value={email}
              onChange={handleChangeEmail}
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
            <Button
              type="submit"
              fullWidth
              variant="contained"
              sx={{ mt: 3, mb: 2 }}
            >
                Log In
            </Button>
            <Grid container>
              <Grid item>
                <Link href="/sign-up" variant="body2">
                  {"Don't have an account? Sign Up"}
                </Link>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Container>
    );
}
