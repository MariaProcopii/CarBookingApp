import React, { useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField,
         Grid, FormControl, InputLabel, Select, MenuItem, Button, Typography, IconButton, InputAdornment } from '@mui/material';
import { MuiTelInput } from 'mui-tel-input';
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DateField } from '@mui/x-date-pickers/DateField';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from "yup";
import axios from 'axios';
import { transformDate } from '../../utils/DateTimeUtils';
import dayjs from 'dayjs';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { useAuth } from '../../components/provider/AuthProvider';
import { useTokenDecoder, hasRole } from '../../utils/TokenUtils';
import { parseErrorMessages } from '../../utils/ErrorUtils';

export default function EditUserDialog({ open, setOpen, userInfo, setUserInfo }) {

  const [phone, setPhone] = useState(userInfo.phoneNumber);
  const [isPhoneValid, setPhoneValid] = useState(true);
  const [date, setDate] = useState(dayjs(userInfo.dateOfBirth));
  const [isDateValid, setDateValid] = useState(true);
  const [showPassword, setShowPassword] = useState(false);
  const [backendErrors, setBackendErrors] = useState({});
  const { token, setToken } = useAuth();
  const claims = useTokenDecoder(token);

  const initialValues = {
    firstName: userInfo.firstName,
    lastName: userInfo.lastName,
    email: userInfo.email,
    gender: userInfo.gender,
    password: "",
    yearsOfExperience: userInfo.yearsOfExperience
  };

  const validationSchema = Yup.object().shape({
    firstName: Yup.string().required("First Name is required"),
    lastName: Yup.string().required("Last Name is required"),
    email: Yup.string().required("Email is required").email("Email is invalid"),
    yearsOfExperience: Yup.string().required("Years of experience are required").min(1, "Years of experience must be greater than 0"),
    password: Yup.string().matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/, "Must contain at least one uppercase letter, one lowercase letter, and one digit with at least 8 characters"),
  });

  const handlePhoneChange = (newPhone) => {
    setPhone(newPhone.replace(/\s+/g, ''));
  };

  const handleChangeDate = (newDate) => {
    setDate(newDate);
  };

  const handleClose = () => {
    setOpen(false);
    setBackendErrors({});
  };

  const checkPhoneValid = () => {
    const re = /^\+\d{10,15}$/;
    setPhoneValid(re.test(phone));
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
    
    handleUpdateUser({
      ...values,
      phoneNumber: phone,
      dateOfBirth: transformDate(date),
    });
  };

  const handleUpdateUser = (updatedInfo) => {
    axios.put(`http://192.168.0.9:5239/user/info/update/${claims.nameidentifier}`, updatedInfo)
      .then(response => {
        setUserInfo(updatedInfo);
        setToken(response.data);
        handleClose();
      })
      .catch((error) => {
        const { data } = error.response;
        setBackendErrors(parseErrorMessages(data.Message));
      });
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
      <DialogTitle>Edit User Info</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Update the user information below:
        </DialogContentText>
        <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
          {(props) => (
            <Form>
              <Grid container spacing={2} sx={{ mt: 1 }}>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="First Name"
                    name="firstName"
                    fullWidth
                    variant="outlined"
                    helperText={<ErrorMessage name="firstName" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Last Name"
                    name="lastName"
                    fullWidth
                    variant="outlined"
                    helperText={<ErrorMessage name="lastName" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Email"
                    type="email"
                    name="email"
                    fullWidth
                    variant="outlined"
                    helperText={<ErrorMessage name="email" />}
                  />
                  {backendErrors.email && (
                    <Typography color="gray" variant="caption" sx={{ml:2}}>
                        {backendErrors.email}
                    </Typography>
                  )}
                </Grid>
                <Grid item xs={12}>
                  <MuiTelInput
                    value={phone}
                    onChange={handlePhoneChange}
                    label="Phone Number"
                    name="phone"
                    autoComplete="phone" 
                    fullWidth
                    required
                  />
                  {!isPhoneValid && (
                    <Typography color="error" variant="caption">
                      Phone is invalid
                    </Typography>
                  )}                                    
                  {backendErrors.phone && (
                      <Typography color="gray" variant="caption" sx={{ml:2}}>
                          {backendErrors.phone}
                      </Typography>
                  )}
                </Grid>
                <Grid item xs={12}>
                  <LocalizationProvider dateAdapter={AdapterDayjs}>
                    <DemoContainer components={['DateField']}>
                      <DateField
                        label="Pick your birthday"
                        name="birthday"
                        value={date}
                        onChange={handleChangeDate}
                        required
                        fullWidth
                      />
                    </DemoContainer>
                  </LocalizationProvider>
                  {!isDateValid && (
                    <Typography color="error" variant="caption">
                      Date is invalid
                    </Typography>
                  )}
                </Grid>
                <Grid item xs={12}>
                  <FormControl fullWidth sx={{ marginTop: 1 }}>
                    <InputLabel id="gender-select-label">Gender</InputLabel>
                    <Field as={Select}
                      labelId="gender-select-label"
                      name="gender"
                      fullWidth
                    >
                      <MenuItem value={"FEMALE"}>Female</MenuItem>
                      <MenuItem value={"MALE"}>Male</MenuItem>
                      <MenuItem value={"OTHER"}>Other</MenuItem>
                    </Field>
                  </FormControl>
                </Grid>
                {hasRole(claims, "Driver") && (
                  <Grid item xs={12}>
                    <Field
                      as={TextField}
                      required
                      margin="dense"
                      label="Years of experience"
                      name="yearsOfExperience"
                      fullWidth
                      variant="outlined"
                      helperText={<ErrorMessage name="yearsOfExperience" />}
                    />
                  </Grid>
                )}
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Password"
                    type={showPassword ? 'text' : 'password'}
                    name="password"
                    fullWidth
                    variant="outlined"
                    helperText={<ErrorMessage name="password" />}
                    InputProps={{
                      endAdornment: (
                        <InputAdornment position="end">
                          <IconButton
                            aria-label="toggle password visibility"
                            onClick={() => setShowPassword(!showPassword)}
                            edge="end"
                          >
                            {showPassword ? <VisibilityOffIcon /> : <VisibilityIcon />}
                          </IconButton>
                        </InputAdornment>
                      ),
                    }}
                  />
                </Grid>
                <DialogActions>
                  <Button onClick={handleClose} color="secondary">Cancel</Button>
                  <Button type="submit" color="primary" >
                    Save
                  </Button>
                </DialogActions>
              </Grid>
            </Form>
          )}
        </Formik>
      </DialogContent>
    </Dialog>
  );
}