import * as Yup from "yup";
import useAuth from "../../context/auth/UseAuth";
import useError from "../../context/error/UseError";
import useAPI from "../../context/api/UseAPI";
import UserService from "../../services/userService/UserService";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import { useState } from "react";
import { useTheme } from "@mui/material";
import { LoginParams } from "../../services/userService/types";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import {
    Form, Formik, Field,
    ErrorMessage, FormikHelpers,
} from "formik";
import {
    Grid, Paper, Avatar,
    TextField, Button,
    Typography, Link, Box,
    CssBaseline, InputAdornment,
    IconButton
} from "@mui/material";

export default function Login() {
    const theme = useTheme();

    const boxStyle = {
        margin: "30px auto",
        alignItems: "center",
        minWidth: 300,
    };
    const paperStyle = {
        padding: 40,
        height: "60vh",
        width: 400,
        margin: "50px auto",
    };
    const avatarStyle = {
        bgcolor: "primary.main",
    };
    const btnstyle = {
        margin: "8px 0",
    };
    const imageStyle = {
        margin: "30px auto",
        maxWidth: "100%",
        height: "auto",
        [theme.breakpoints.down("sm")]: {
            width: "100%",
        },
        [theme.breakpoints.up("sm")]: {
            width: "80%",
        },
        [theme.breakpoints.up("md")]: {
            width: "60%",
        },
        [theme.breakpoints.up("lg")]: {
            width: "50%",
        },
    };

    const [showPassword, setShowPassword] = useState(false);

    const { setToken } = useAuth();
    const { error } = useError();

    const { instance } = useAPI();
    const { login } = UserService(instance);

    const initialValues = {
        email: "",
        password: ""
    };

    const handleClickShowPassword = () => {
        setShowPassword(!showPassword);
    };

    const handleMouseDownPassword = (event: any) => {
        event.preventDefault();
    };

    const getImagePath = () => {
        if (theme.palette.mode === "dark") {
            return "src/assets/images/car-intro-dark.png";
        } else {
            return "src/assets/images/car-intro-light.png";
        }
    };

    const onSubmit = async (
        values: LoginParams,
        formikHelpers: FormikHelpers<LoginParams>
    ) => {
        await login({
            email: values.email,
            password: values.password
        }, setToken);

        setTimeout(() => {
            formikHelpers.resetForm();
            formikHelpers.setSubmitting(false);
        }, 1000);
    };

    const validationSchema = Yup.object().shape({
        email: Yup.string()
            .required("Email is required")
            .email("Email is invalid"),
        password: Yup.string()
            .required("Password is required")
            .matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/, "Must contain at least one uppercase letter, one lowercase letter, and one digit with at least 8 characters")
    });

    return (
        <>
            <CssBaseline />
            <Grid container>
                <Box sx={boxStyle} >
                    <img src={getImagePath()} style={imageStyle} />
                </Box>
                <Paper elevation={8} style={paperStyle}>
                    <Grid
                        container
                        direction="column"
                        justifyContent="center"
                        alignItems="center"
                    >
                        <Avatar sx={avatarStyle}><LockOutlinedIcon /></Avatar>
                        <h2>Sign In</h2>
                    </Grid>
                    <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
                        {(props) => (
                            <Form>
                                <Field
                                    as={TextField}
                                    label="Email"
                                    name="email"
                                    sx={{ mt: 2 }}
                                    placeholder="Enter email"
                                    type="email"
                                    fullWidth
                                    required
                                    helperText={<ErrorMessage name="email" />}
                                />
                                {error.current.email && (
                                    <Typography color="error" variant="caption" sx={{ ml: 2 }}>
                                        {error.current.email}
                                    </Typography>
                                )}
                                <Field
                                    as={TextField}
                                    label="Password"
                                    name="password"
                                    sx={{ mt: 2 }}
                                    placeholder="Enter password"
                                    type={showPassword ? "text" : "password"}
                                    fullWidth
                                    required
                                    helperText={<ErrorMessage name="password" />}
                                    InputProps={{
                                        endAdornment:
                                            (
                                                <InputAdornment position="end">
                                                    <IconButton
                                                        aria-label="toggle password visibility"
                                                        onClick={handleClickShowPassword}
                                                        onMouseDown={handleMouseDownPassword}
                                                    >
                                                        {showPassword ? <Visibility /> : <VisibilityOff />}
                                                    </IconButton>
                                                </InputAdornment>
                                            ),
                                    }}
                                />
                                {error.current.password && (
                                    <Typography color="error" variant="caption" sx={{ ml: 2 }}>
                                        {error.current.password}
                                    </Typography>
                                )}
                                <Button
                                    type="submit"
                                    color="primary"
                                    variant="contained"
                                    style={btnstyle}
                                    fullWidth
                                    disabled={props.isSubmitting}
                                >{props.isSubmitting ? "Loading" : "Log in"}</Button>
                            </Form>
                        )}
                    </Formik>
                    <Box sx={{ mt: 2 }}></Box>
                    <Typography > 
                        {" "}
                        Do you want to create an account ?
                        <Link href="/signup" >
                            Sign Up Here
                        </Link>
                    </Typography>
                </Paper>
            </Grid>
        </>
    );
}