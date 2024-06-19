import * as Yup from "yup";
import dayjs from "dayjs";
import LockOutlinedIcon from "@mui/icons-material/LockOutlined";
import useAuth from "../../context/auth/UseAuth";
import useError from "../../context/error/UseError";
import useAPI from "../../context/api/UseAPI";
import UserService from "../../services/userService/UserService";
import { useState } from "react";
import { MuiTelInput } from "mui-tel-input";
import { DateField } from "@mui/x-date-pickers/DateField";
import { RegisterParams } from "../../services/userService/types";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import {
    Form, Formik, FormikHelpers,
    Field, ErrorMessage
} from "formik";
import {
    Grid, Paper, Avatar, TextField,
    Button, Typography, Link, Box, useTheme,
    FormControl, InputLabel, Select, MenuItem,
    CssBaseline
} from "@mui/material";

export default function SignUp() {
    const theme = useTheme();

    const boxStyle = {
        margin: "40px auto",
        alignItems: "center",
        minWidth: 300,
    };
    const paperStyle = {
        padding: 40,
        height: "75vh",
        width: 600,
        margin: "50px auto"
    };
    const avatarStyle = {
        bgcolor: "primary.main"
    };
    const btnstyle = {
        margin: "8px 0"
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

    const initialValues = {
        firstName: "",
        lastName: "",
        email: "",
        gender: "OTHER",
        password: "",
        confirm_password: "",
        phoneNumber: "",
        dateOfBirth: "",
    };

    const [phone, setPhone] = useState("");
    const [isPhoneValid, setPhoneValid] = useState(true);
    const [isDateValid, setDateValid] = useState(true);
    const [date, setDate] = useState(dayjs().format("YYYY-MM-DD"));

    const { setToken } = useAuth();
    const { error } = useError();

    const { instance } = useAPI();
    const { signup } = UserService(instance);

    const getImagePath = () => {
        if (theme.palette.mode === "dark") {
            return "src/assets/images/car-intro-dark.png";
        } else {
            return "src/assets/images/car-intro-light.png";
        }
    };

    const handlePhoneChange = (newPhone: string) => {
        setPhone(newPhone.replace(/\s+/g, ""));
    };

    const checkPhoneValid = () => {
        const re = /^\+\d{10,15}$/;

        if (re.test(phone)) {
            setPhoneValid(true);
        }
        else {
            setPhoneValid(false);
        }
    };

    const checkDateValid = () => {
        const re = /^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/;

        if (re.test(date)) {
            setDateValid(true);
        }
        else {
            setDateValid(false);
        }
    };

    const onSubmit = async (
        values: RegisterParams,
        formikHelpers: FormikHelpers<RegisterParams>,
    ) => {
        checkDateValid();
        checkPhoneValid();

        if (!isDateValid || !isPhoneValid) {
            return;
        }

        await signup({
            ...values,
            phoneNumber: phone,
            dateOfBirth: date,
        }, setToken);

        setTimeout(() => {
            formikHelpers.setSubmitting(false);
        }, 1000);
    };

    const validationSchema = Yup.object().shape({
        firstName: Yup.string()
            .required("First Name is required"),
        lastName: Yup.string()
            .required("Last Name is required"),
        email: Yup.string()
            .required("Email is required")
            .email("Email is invalid"),
        password: Yup.string()
            .required("Password is required")
            .matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/, "Must contain at least one uppercase letter, one lowercase letter, and one digit with at least 8 characters"),
        confirm_password: Yup.string()
            .oneOf([Yup.ref("password"), ""], "Passwords must match")
            .required("Confirm password is required")
    });

    return (
        <>
            <CssBaseline />
            <Grid container>
                <Box sx={boxStyle} >
                    <img src={getImagePath()} style={imageStyle} />
                </Box>
                <Paper elevation={8} style={paperStyle} sx={{ width: "800px" }}>
                    <Grid container direction="column" justifyContent="center" alignItems="center">
                        <Avatar sx={avatarStyle}><LockOutlinedIcon /></Avatar>
                        <h2>Sign Up</h2>
                    </Grid>
                    <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
                        {(props) => (
                            <Form>
                                <Grid container spacing={2}>
                                    <Grid xs={6} item>
                                        <Field
                                            as={TextField}
                                            label="First Name"
                                            name="firstName"
                                            placeholder="Enter first name"
                                            fullWidth
                                            required
                                            inputProps={{ maxLength: 30 }}
                                            helperText={<ErrorMessage name="firstName" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field
                                            as={TextField}
                                            label="Last Name"
                                            name="lastName"
                                            placeholder="Enter last name"
                                            fullWidth
                                            required
                                            inputProps={{ maxLength: 30 }}
                                            helperText={<ErrorMessage name="lastName" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field
                                            as={TextField}
                                            label="Email"
                                            name="email"
                                            placeholder="Enter email"
                                            type="email"
                                            fullWidth
                                            required
                                            helperText={<ErrorMessage name="email" />}
                                        />
                                        {error.current.email && (
                                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                                {error.current.email}
                                            </Typography>
                                        )}
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field as={MuiTelInput}
                                            value={phone}
                                            onChange={handlePhoneChange}
                                            label="Phone Number"
                                            name="phone"
                                            autoComplete="phone"
                                            placeholder="Enter phone number"
                                            fullWidth
                                            required
                                            inputProps={{ maxLength: 20 }}
                                        />
                                        {error.current.phone && (
                                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                                {error.current.phone}
                                            </Typography>
                                        )}
                                        {!isPhoneValid && (
                                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                                Phone is invalid
                                            </Typography>
                                        )}
                                    </Grid>
                                    <Grid xs={6} item>
                                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                                            <DemoContainer components={["DateField"]}>
                                                <DateField
                                                    label="Pick your birthday"
                                                    name="birthday"
                                                    value={dayjs(date)}
                                                    onChange={(datejs: dayjs.Dayjs | null) => setDate(datejs ? datejs.format("YYYY-MM-DD") : "")}
                                                    required
                                                    fullWidth
                                                    sx={{ mb: 2 }}
                                                />
                                            </DemoContainer>
                                        </LocalizationProvider>
                                        {error.current.birthday && (
                                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                                {error.current.birthday}
                                            </Typography>
                                        )}
                                        {!isDateValid && (
                                            <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                                Date is invalid
                                            </Typography>
                                        )}
                                    </Grid>
                                    <Grid xs={6} item>
                                        <FormControl
                                            fullWidth
                                            sx={{ marginTop: 1 }}
                                        >
                                            <InputLabel id="gender-select-label">Gender</InputLabel>
                                            <Field as={Select}
                                                name="gender"
                                                label="Gender"
                                                required
                                            >
                                                <MenuItem value={"FEMALE"}>Female</MenuItem>
                                                <MenuItem value={"MALE"}>Male</MenuItem>
                                                <MenuItem value={"OTHER"}>Other</MenuItem>
                                            </Field>
                                        </FormControl>
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field
                                            as={TextField}
                                            label="Password"
                                            name="password"
                                            placeholder="Enter password"
                                            type="password"
                                            fullWidth
                                            required
                                            inputProps={{ maxLength: 30 }}
                                            helperText={<ErrorMessage name="password" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field
                                            as={TextField}
                                            label="Confirm password"
                                            name="confirm_password"
                                            placeholder="Confirm password"
                                            type="password"
                                            fullWidth
                                            required
                                            inputProps={{ maxLength: 30 }}
                                            helperText={<ErrorMessage name="confirm_password" />}
                                        />
                                    </Grid>
                                    <Grid xs={12} item>
                                        <Button
                                            type="submit"
                                            color="primary"
                                            variant="contained"
                                            style={btnstyle}
                                            fullWidth
                                            disabled={props.isSubmitting}
                                        >{props.isSubmitting ? "Loading" : "Sign up"}
                                        </Button>
                                    </Grid>
                                </Grid>
                            </Form>
                        )}
                    </Formik>
                    <Box sx={{ mt: 2 }}></Box>
                    <Typography > Already have an account ?
                        <Link href="/login" >
                            Log In Here
                        </Link>
                    </Typography>
                </Paper>
            </Grid>
        </>
    );
}