import * as Yup from "yup";
import useAuth from "../../context/auth/UseAuth";
import useError from "../../context/error/UseError";
import useAPI from "../../context/api/UseAPI";
import RideService from "../../services/rideService/RideService";
import { useTokenDecoder } from "../../utils/TokenUtils";
import { useState, useEffect } from "react";
import {
    Form, Formik,
    Field, ErrorMessage
} from "formik";
import {
    Dialog, DialogActions, DialogContent,
    DialogTitle, Button, Stepper, Step,
    StepLabel, StepContent, Box, TextField,
    Typography, MenuItem, Paper,
    StepLabelProps
} from "@mui/material";

interface Props {
    open: any;
    setOpen: any;
    vehicleDetail: any;
    setVehicleDetail: any;
}

export default function EditDriverDialog({
    open,
    setOpen,
    vehicleDetail,
    setVehicleDetail
}: Props) {
    const steps = ["Vehicle Details", "Vendor", "Model"];

    const [activeStep, setActiveStep] = useState(0);
    const [vendors, setVendors] = useState([]);
    const [models, setModels] = useState([]);

    const { error, setError } = useError();

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const {
        fetchVendors,
        fetchModels,
        updateVehicleDetails,
    } = RideService(instance);

    const initialValues = {
        manufactureYear: vehicleDetail.manufactureYear,
        registrationNumber: vehicleDetail.registrationNumber,
        vender: vehicleDetail.vehicle.vender,
        model: vehicleDetail.vehicle.model,
    };

    useEffect(() => {
        setTimeout(() => {
            fetchVendors(setVendors);
        }, 10);
    }, []);

    useEffect(() => {
        setTimeout(() => {
            fetchModels(initialValues.vender, setModels);
        }, 10);
    }, []);

    const validationSchemas = [
        Yup.object().shape({
            manufactureYear: Yup.number()
                .required("Manufacture Year is required")
                .min(1990, "Year must be greather than 1990"),
            registrationNumber: Yup.string()
                .required("Registration Number is required")
                .matches(/^[A-Z]{3} [0-9]{3}$/, "Invalid registration number format. Example: ABC 123")
        }),
        Yup.object().shape({
            vender: Yup.string()
                .required("Vendor is required")
                .max(50, "Maximum length is 50 characters"),
        }),
        Yup.object().shape({
            model: Yup.string()
                .required("Model is required")
                .max(50, "Maximum length is 50 characters")
        }),
    ];

    const handleClose = () => {
        setOpen(false);
        setActiveStep(0);
        setError("");
    };

    const handleNext = (isValid: boolean) => {
        if (isValid) {
            setActiveStep((prevActiveStep) => prevActiveStep + 1);
        }
    };

    const handleBack = () => {
        setActiveStep((prevActiveStep) => prevActiveStep - 1);
    };

    const handleSubmit = (values: {
        manufactureYear: any;
        registrationNumber: any;
        vender: any;
        model: any;
    }) => {
        const formatedValues = {
            manufactureYear: values.manufactureYear,
            registrationNumber: values.registrationNumber,
            vehicle: {
                vender: values.vender,
                model: values.model
            }
        };

        updateVehicleDetails(
            claims.nameidentifier as string,
            formatedValues,
            setOpen,
            setVehicleDetail
        );
        if (Object.keys(error.current ? error.current : {}).length === 0) {
            handleClose();
        }
    };

    function isStepFailed(step: number) {
        if (step === 0 && (error.current.manufactureYear || error.current.registrationNumber)) return true;
        if (step === 1 && error.current.vender) return true;
        if (step === 2 && error.current.model) return true;
        return false;
    };

    return (
        <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
            <DialogTitle>Edit Vehicle Details</DialogTitle>
            <DialogContent>
                <Formik
                    initialValues={initialValues}
                    validationSchema={validationSchemas[activeStep]}
                    onSubmit={handleSubmit}
                    validateOnChange
                >
                    {({ isValid, setFieldValue }) => (
                        <Form>
                            <Box sx={{ maxWidth: 400 }}>
                                <Stepper activeStep={activeStep} orientation="vertical">
                                    {steps.map((label, index) => {
                                        const labelProps: StepLabelProps = {};
                                        if (error.current && isStepFailed(index)) {
                                            labelProps.optional = (
                                                <Typography variant="caption" color="error">
                                                    {error.current[Object.keys(error.current)[0]]}
                                                </Typography>
                                            );
                                            labelProps.error = true;
                                        }
                                        return (
                                            <Step key={label}>
                                                <StepLabel {...labelProps}>{label}</StepLabel>
                                                <StepContent>
                                                    <Box sx={{ mb: 2 }}>
                                                        {index === 0 && (
                                                            <>
                                                                <Field
                                                                    as={TextField}
                                                                    margin="dense"
                                                                    label="Manufacture Year"
                                                                    name="manufactureYear"
                                                                    fullWidth
                                                                    variant="outlined"
                                                                    helperText={<ErrorMessage name="manufactureYear" />}
                                                                />
                                                                <Field
                                                                    as={TextField}
                                                                    margin="dense"
                                                                    label="Registration Number"
                                                                    name="registrationNumber"
                                                                    fullWidth
                                                                    variant="outlined"
                                                                    helperText={<ErrorMessage name="registrationNumber" />}
                                                                />
                                                            </>
                                                        )}
                                                        {index === 1 && (
                                                            <>
                                                                <Field
                                                                    as={TextField}
                                                                    margin="dense"
                                                                    label="Vendor"
                                                                    name="vender"
                                                                    select
                                                                    fullWidth
                                                                    variant="outlined"
                                                                    helperText={<ErrorMessage name="vender" />}
                                                                    onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
                                                                        const vendor = event.target.value;
                                                                        setFieldValue("vender", vendor);
                                                                        setFieldValue("model", "");
                                                                        fetchModels(vendor, setModels);
                                                                    }}
                                                                >
                                                                    {vendors.map((vendor) => (
                                                                        <MenuItem key={vendor} value={vendor}>
                                                                            {vendor}
                                                                        </MenuItem>
                                                                    ))}
                                                                </Field>
                                                            </>
                                                        )}
                                                        {index === 2 && (
                                                            <>
                                                                <Field
                                                                    as={TextField}
                                                                    margin="dense"
                                                                    label="Model"
                                                                    name="model"
                                                                    select
                                                                    fullWidth
                                                                    variant="outlined"
                                                                    helperText={<ErrorMessage name="model" />}
                                                                >
                                                                    {models.map((model) => (
                                                                        <MenuItem key={model} value={model}>
                                                                            {model}
                                                                        </MenuItem>
                                                                    ))}
                                                                </Field>
                                                            </>
                                                        )}
                                                        <DialogActions>
                                                            {activeStep !== 0 && (
                                                                <Button onClick={handleBack}>
                                                                    Back
                                                                </Button>
                                                            )}
                                                            <Button
                                                                variant="contained"
                                                                onClick={() => handleNext(isValid)}
                                                                disabled={!isValid}
                                                                sx={{ mt: 1, mr: 1 }}
                                                            >
                                                                {index === steps.length - 1 ? "Finish" : "Continue"}
                                                            </Button>
                                                        </DialogActions>
                                                    </Box>
                                                </StepContent>
                                            </Step>
                                        );
                                    })}
                                </Stepper>
                                {activeStep === steps.length && (
                                    <Paper square elevation={0} sx={{ p: 3 }}>
                                        <Typography>All steps completed - save changes ?</Typography>
                                        <Button onClick={handleClose} sx={{ mt: 1, mr: 1 }}>
                                            Close
                                        </Button>
                                        <Button type="submit" sx={{ mt: 1, mr: 1 }}>
                                            Save
                                        </Button>
                                    </Paper>
                                )}
                            </Box>
                        </Form>
                    )}
                </Formik>
            </DialogContent>
        </Dialog>
    );
}