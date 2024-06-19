import * as Yup from "yup";
import dayjs from "dayjs";
import useAuth from "../../context/auth/UseAuth";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import useAPI from "../../context/api/UseAPI";
import useError from "../../context/error/UseError";
import UserService from "../../services/userService/UserService";
import { useState } from "react";
import { MuiTelInput } from "mui-tel-input";
import { DateField } from "@mui/x-date-pickers/DateField";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { useTokenDecoder, hasRole } from "../../utils/TokenUtils";
import { DetailsParams } from "../../services/userService/types";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import {
    Form, Formik,
    Field, ErrorMessage,
    FormikHelpers
} from "formik";
import {
    Dialog, DialogActions, DialogContent,
    DialogContentText, DialogTitle, TextField,
    Grid, FormControl, InputLabel, Select,
    MenuItem, Button, Typography, IconButton, InputAdornment
} from "@mui/material";

interface Props {
    open: any;
    setOpen: any;
    userInfo: any;
    setUserInfo: any;
}

export default function EditUserDialog({ open, setOpen, userInfo, setUserInfo }: Props) {
    const [phone, setPhone] = useState(userInfo.phoneNumber);
    const [isPhoneValid, setPhoneValid] = useState(true);
    const [date, setDate] = useState(dayjs(userInfo.dateOfBirth).format("YYYY-MM-DD"));
    const [isDateValid, setDateValid] = useState(true);
    const [showPassword, setShowPassword] = useState(false);

    const { error, setError } = useError();

    const { token, setToken } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { updateDetails } = UserService(instance);

    const initialValues = {
        firstName: userInfo.firstName,
        lastName: userInfo.lastName,
        email: userInfo.email,
        gender: userInfo.gender,
        password: "",
        yearsOfExperience: userInfo.yearsOfExperience,
        dateOfBirth: userInfo.date,
        phoneNumber: userInfo.phone,
    };

    const validationSchema = Yup.object().shape({
        firstName: Yup.string().required("First Name is required"),
        lastName: Yup.string().required("Last Name is required"),
        email: Yup.string().required("Email is required").email("Email is invalid"),
        yearsOfExperience: Yup.string().required("Years of experience are required").min(1, "Years of experience must be greater than 0"),
        password: Yup.string().matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/, "Must contain at least one uppercase letter, one lowercase letter, and one digit with at least 8 characters"),
    });

    const handlePhoneChange = (newPhone: string) => {
        setPhone(newPhone.replace(/\s+/g, ""));
    };

    const handleClose = () => {
        setOpen(false);
        setError("");
    };

    const checkPhoneValid = () => {
        const re = /^\+\d{10,15}$/;
        setPhoneValid(re.test(phone));
    };

    const checkDateValid = () => {
        const re = /^\d{4}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01])$/;

        if (re.test(date)) {
            setDateValid(true);
        } else {
            setDateValid(false);
        }
    };

    const onSubmit = async (
        values: DetailsParams,
        _: FormikHelpers<DetailsParams>
    ) => {
        checkDateValid();
        checkPhoneValid();
        if (!isDateValid || !isPhoneValid) {
            return;
        }

        await updateDetails(
            claims.nameidentifier as string,
            {
                ...values,
                phoneNumber: phone,
                dateOfBirth: date,
            },
            setUserInfo,
            setToken,
            handleClose,
        );
    };

    return (
        <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
            <DialogTitle>Edit User Info</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Update the user information below:
                </DialogContentText>
                <Formik initialValues={initialValues} onSubmit={onSubmit} validationSchema={validationSchema}>
                    {() => (
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
                                    {error.current.email && (
                                        <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                            {error.current.email}
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
                                    {error.current.phone && (
                                        <Typography color="gray" variant="caption" sx={{ ml: 2 }}>
                                            {error.current.phone}
                                        </Typography>
                                    )}
                                </Grid>
                                <Grid item xs={12}>
                                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                                        <DemoContainer components={["DateField"]}>
                                            <DateField
                                                label="Pick your birthday"
                                                name="birthday"
                                                value={dayjs(date)}
                                                onChange={(datejs) => setDate(datejs ? datejs.format("YYYY-MM-DD") : "")}
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
                                        type={showPassword ? "text" : "password"}
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