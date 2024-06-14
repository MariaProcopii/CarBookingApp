import { Grid,Paper, Avatar, TextField, Button, Typography, Link, Box, FormControl,
         InputLabel, Select, MenuItem, CssBaseline}  from '@mui/material';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateField } from '@mui/x-date-pickers/DateField';
import { MuiTelInput } from 'mui-tel-input';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from "yup";
import axios from 'axios';
import {useState} from 'react';
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../components/provider/AuthProvider";
import { parseErrorMessages } from '../../utils/ErrorUtils';
import { transformDate } from '../../utils/DateTimeUtils';

export default function SignUp() {
    const boxStyle={ margin:"40px auto", alignItems: 'center', minWidth:300 };
    const paperStyle={padding :40,height:'75vh',width:600, margin:"50px auto"};
    const avatarStyle={ bgcolor: 'primary.main' };
    const btnstyle={margin:'8px 0'};
    const imageStyle={
        margin: "30px auto",
        maxWidth: '100%',
        height: 'auto',
        width: '100%',
        maxHeight: {
            xs: '200px',
            sm: '250px',
            md: '300px',
            lg: '350px',
            xl: '400px',
        },
    };

    const { setToken } = useAuth();
    const [backendErrors, setBackendErrors] = useState({});
    const navigate = useNavigate();
    const initialValues = {
        firstName: "",
        lastName: "",
        email: "",
        gender: "OTHER",
        password: "",
        confirm_password: ""
    };
    const [phone, setPhone] = useState('');
    const [isPhoneValid, setPhoneValid] = useState(true);
    const [date, setDate] = useState(null);
    const [isDateValid, setDateValid] = useState(true);

    const handlePhoneChange = (newPhone) => {
        setPhone(newPhone.replace(/\s+/g, ''));
    };

    const handleChangeDate = (date) => {
        setDate(date);
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
        const birthday = transformDate(date);

        if (re.test(birthday)) {
            setDateValid(true);
        }
        else {
            setDateValid(false);
        }
    };

    const onSubmit = (values, props) => {
        checkDateValid();
        checkPhoneValid();
        if (!isDateValid || !isPhoneValid) {
            return;
        }

        axios.post(
            "http://localhost:5239/Auth/SignUp",
            {
              ...values,
              phoneNumber: phone,
              dateOfBirth: transformDate(date)
            },
          )
          .then((response) => {
            setToken(response.data);
            navigate("/", { replace: true});
          })
          .catch((error) => {
            const { data } = error.response;
            setBackendErrors(parseErrorMessages(data.Message));
          });

        setTimeout(()=>{
            props.setSubmitting(false);
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
            .oneOf([Yup.ref('password'), null], "Passwords must match")
            .required("Confirm password is required")
    });

    return(
        <>
        <CssBaseline />
            <Grid container>
                <Box sx={boxStyle} >
                    <img src="src/assets/images/car-intro.png" style={imageStyle}/>
                </Box>
                <Paper elevation={8} style={paperStyle} sx={{width: '800px'}}>
                    <Grid align='center'>
                        <Avatar sx={avatarStyle}><LockOutlinedIcon/></Avatar>
                        <h2>Sign Un</h2>
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
                                            placeholder='Enter first name' 
                                            fullWidth 
                                            required
                                            inputProps={{maxLength: 30}}
                                            helperText={<ErrorMessage name="firstName" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field 
                                            as={TextField} 
                                            label="Last Name"
                                            name="lastName"
                                            placeholder='Enter last name' 
                                            fullWidth 
                                            required
                                            inputProps={{maxLength: 30}}
                                            helperText={<ErrorMessage name="lastName" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field 
                                            as={TextField} 
                                            label='Email' 
                                            name="email" 
                                            placeholder='Enter email' 
                                            type='email' 
                                            fullWidth 
                                            required
                                            helperText={<ErrorMessage name="email" />}
                                        />
                                        {backendErrors.email && (
                                            <Typography color="gray" variant="caption" sx={{ml:2}}>
                                                {backendErrors.email}
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
                                            placeholder='Enter phone number'
                                            fullWidth 
                                            required
                                            inputProps={{maxLength: 20}}
                                        />
                                        {backendErrors.phone && (
                                            <Typography color="gray" variant="caption" sx={{ml:2}}>
                                                {backendErrors.phone}
                                            </Typography>
                                        )}
                                        {!isPhoneValid && (
                                            <Typography color="gray" variant="caption" sx={{ml:2}}>
                                                Phone is invalid
                                            </Typography>
                                        )}
                                    </Grid>
                                    <Grid xs={6} item>
                                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                                            <DemoContainer components={['DateField']}>
                                                <Field as={DateField}
                                                    label="Pick your birthday"
                                                    name="birthday"
                                                    value={date}
                                                    onChange={handleChangeDate}
                                                    required
                                                    fullWidth
                                                    sx={{ mb: 2 }}
                                                />
                                            </DemoContainer>
                                        </LocalizationProvider>
                                        {backendErrors.birthday && (
                                            <Typography color="gray" variant="caption" sx={{ml:2}}>
                                                {backendErrors.birthday}
                                            </Typography>
                                        )}
                                        {!isDateValid && (
                                            <Typography color="gray" variant="caption" sx={{ml:2}}>
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
                                            label='Password' 
                                            name="password" 
                                            placeholder='Enter password' 
                                            type='password' 
                                            fullWidth 
                                            required
                                            inputProps={{maxLength: 30}}
                                            helperText={<ErrorMessage name="password" />}
                                        />
                                    </Grid>
                                    <Grid xs={6} item>
                                        <Field 
                                            as={TextField} 
                                            label='Confirm password' 
                                            name="confirm_password" 
                                            placeholder='Confirm password' 
                                            type='password' 
                                            fullWidth 
                                            required
                                            inputProps={{maxLength: 30}}
                                            helperText={<ErrorMessage name="confirm_password" />}
                                        />
                                    </Grid>
                                    <Grid xs={12} item>
                                        <Button 
                                            type='submit' 
                                            color='primary' 
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
                    <Box sx={{mt:2}}></Box>
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