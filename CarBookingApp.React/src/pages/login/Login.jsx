import { Grid, Paper, Avatar, TextField, Button, Typography, Link, Box, InputAdornment, IconButton } from '@mui/material';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from "yup";
import axios from 'axios';
import { useState } from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/provider/AuthProvider";
import { parseErrorMessages } from '../../utils/ErrorUtils';

export default function Login() {
    const boxStyle = { margin: "30px auto", alignItems: 'center', minWidth: 300 };
    const paperStyle = { padding: 40, height: '50vh', width: 400, margin: "50px auto" };
    const avatarStyle = { bgcolor: 'primary.main' };
    const imageStyle = {
        margin: "30px auto",
        maxWidth: '100%',
        height: 'auto',
        width: '100%',
        maxHeight: {
            xs: '200px',
            sm: '250px',
            md: '300px',
            lg: '300px',
            xl: '300px',
        },
    };

    const btnstyle = { margin: '8px 0' };
    const { setToken } = useAuth();
    const [backendErrors, setBackendErrors] = useState({});
    const navigate = useNavigate();
    const initialValues = {
        email: "",
        password: ""
    };
    const [showPassword, setShowPassword] = useState(false);

    const handleClickShowPassword = () => {
        setShowPassword(!showPassword);
    };

    const handleMouseDownPassword = (event) => {
        event.preventDefault();
    };

    const onSubmit = (values, props) => {
        axios.post(
            "http://192.168.0.9:5239/Auth/LogIn",
            {
                email: values.email,
                password: values.password,
            },
        )
            .then((response) => {
                setToken(response.data);
                navigate("/", { replace: true });
            })
            .catch((error) => {
                const { data } = error.response;
                setBackendErrors(parseErrorMessages(data.Message));
            });

        setTimeout(() => {
            props.resetForm();
            props.setSubmitting(false);
        }, 10);
    };

    const validationSchema = Yup.object().shape({
        email: Yup.string()
            .required("Email is required")
            .email("Email is invalid"),
        password: Yup.string()
            .required("Password is required")
            .matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/, 'Must contain at least one uppercase letter, one lowercase letter, and one digit with at least 8 characters')
    });

    return (
        <Grid container>
            <Box sx={boxStyle} >
                <img src="src/assets/images/car-intro.png" style={imageStyle}/>
            </Box>
            <Paper elevation={8} style={paperStyle}>
                <Grid align='center'>
                    <Avatar sx={avatarStyle}><LockOutlinedIcon /></Avatar>
                    <h2>Sign In</h2>
                </Grid>
                <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
                    {(props) => (
                        <Form>
                            <Field
                                as={TextField}
                                label='Email'
                                name="email"
                                sx={{ mt: 2 }}
                                placeholder='Enter email'
                                type='email'
                                fullWidth
                                required
                                helperText={<ErrorMessage name="email" />}
                            />
                            {backendErrors.email && (
                                <Typography color="error" variant="caption" sx={{ ml: 2 }}>
                                    {backendErrors.email}
                                </Typography>
                            )}
                            <Field
                                as={TextField}
                                label='Password'
                                name="password"
                                sx={{ mt: 2 }}
                                placeholder='Enter password'
                                type={showPassword ? 'text' : 'password'}
                                fullWidth
                                required
                                helperText={<ErrorMessage name="password" />}
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton
                                                aria-label="toggle password visibility"
                                                onClick={handleClickShowPassword}
                                                onMouseDown={handleMouseDownPassword}
                                            >
                                                {showPassword ? <Visibility /> : <VisibilityOff />}
                                            </IconButton>
                                        </InputAdornment>
                                    )
                                }}
                            />
                            {backendErrors.password && (
                                <Typography color="error" variant="caption" sx={{ ml: 2 }}>
                                    {backendErrors.password}
                                </Typography>
                            )}
                            <Button
                                type='submit'
                                color='primary'
                                variant="contained"
                                style={btnstyle}
                                fullWidth
                                disabled={props.isSubmitting}
                            >{props.isSubmitting ? "Loading" : "Log in"}</Button>
                        </Form>
                    )}
                </Formik>
                <Box sx={{ mt: 2 }}></Box>
                <Typography > Do you want to create an account ?
                    <Link href="/signup" >
                        Sign Up Here
                    </Link>
                </Typography>
            </Paper>
        </Grid>
    );
}