import React, { useState, useEffect } from 'react';
import {
  Dialog, DialogActions, DialogContent, DialogTitle, Button, Stepper, Step, StepLabel, StepContent, Box,
  TextField, Typography, MenuItem, CircularProgress, Paper
} from '@mui/material';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';
import { useAuth } from '../provider/AuthProvider';
import { useTokenDecoder } from '../../utils/TokenUtils';
import { parseErrorMessages } from '../../utils/ErrorUtils';

const steps = ['Years of Experience', 'Vehicle Details', 'Vendor', 'Model'];

export default function UpgradeUserDialog({ open, setOpen, setVehicleDetail, userInfo, setUserInfo }) {
  const [activeStep, setActiveStep] = useState(0);
  const [vendors, setVendors] = useState([]);
  const [models, setModels] = useState([]);
  const { token, setToken } = useAuth();
  const claims = useTokenDecoder(token);
  const [backendErrors, setBackendErrors] = useState({});

  const initialValues = {
    yearsOfExperience: 1,
    manufactureYear: 2024,
    registrationNumber: "ABC 123",
    vender: "Toyota",
    model: "Corolla"
  };

  useEffect(() => {
    fetchVendors();
  }, []);

  useEffect(() => {
    setTimeout(() => {
        fetchModels(initialValues.vender);
      }, 10);
  }, []);

  const fetchVendors = () => {
    axios.get("http://192.168.0.9:5239/vehicle/pick/vendor")
      .then((response) => {
        setVendors(response.data);
      })
      .catch((error) => {
        console.error('Error fetching vendors:', error);
      });
  };

  const fetchModels = (vendorName) => {
    axios.get(`http://192.168.0.9:5239/vehicle/pick/model?vendor=${vendorName}`)
      .then((response) => {
        setModels(response.data);
      })
      .catch((error) => {
        console.error('Error fetching models:', error);
      });
  };

  const upgradeUser = (yearsOfExperience) => {
    axios
      .post(`http://192.168.0.9:5239/user/info/upgrade/${claims.nameidentifier}`, yearsOfExperience)
      .then((response) => {
        setToken(response.data);
        setUserInfo({
          ...userInfo,
          ...yearsOfExperience
        });
      })
      .catch((error) => {
        console.error(error.data);
      });
  };

  const createVehicleDetail = (vehicleDetail) => {
    axios
      .post(`http://192.168.0.9:5239/vehicledetail/create/${claims.nameidentifier}`, vehicleDetail)
      .then((response) => {
        setVehicleDetail(response.data);
      })
      .catch((error) => {
        const { data } = error.response;
        setBackendErrors(parseErrorMessages(data.Message));
      });
  };

  const validationSchemas = [
    Yup.object().shape({
      yearsOfExperience: Yup.number()
        .required("Years of experience are required")
        .min(1, "Years of experience must be greater than 0"),
    }),
    Yup.object().shape({
      manufactureYear: Yup.number()
        .required("Manufacture Year is required")
        .min(1990, "Year must be greater than 1990"),
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
    })
  ];

  const handleNext = (isValid) => {
    if (isValid) {
      setActiveStep((prevActiveStep) => prevActiveStep + 1);
    }
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const handleClose = () => {
    setOpen(false);
    setActiveStep(0);
    setBackendErrors({});
  };

  const handleSubmit = (values) => {
    const vehicleDetail = {
      manufactureYear: values.manufactureYear,
      registrationNumber: values.registrationNumber,
      vehicle: {
        vender: values.vender,
        model: values.model
      }
    };

    const yearsOfExperience = {
      yearsOfExperience: values.yearsOfExperience
    }

    createVehicleDetail(vehicleDetail);
    upgradeUser(yearsOfExperience);
    handleClose();
  };

  function isStepFailed(step) {
    if (step === 0 && backendErrors.yearsOfExperience) return true;
    if (step === 1 && (backendErrors.manufactureYear || backendErrors.registrationNumber)) return true;
    if (step === 2 && backendErrors.vender) return true;
    if (step === 3 && backendErrors.model) return true;
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
          {({ isValid, values, dirty, setFieldValue }) => (
            <Form>
              <Box sx={{ maxWidth: 400 }}>
                <Stepper activeStep={activeStep} orientation="vertical">
                  {steps.map((label, index) => {
                    const labelProps = {};
                    if (backendErrors && isStepFailed(index)) {
                      labelProps.optional = (
                        <Typography variant="caption" color="error">
                          {backendErrors[Object.keys(backendErrors)[0]]}
                        </Typography>
                      );
                      setOpen(true);
                      labelProps.error = true;
                    }
                    return (
                      <Step key={label}>
                        <StepLabel {...labelProps}>{label}</StepLabel>
                        <StepContent>
                          <Box sx={{ mb: 2 }}>
                            {index === 0 && (
                              <Field
                                as={TextField}
                                margin="dense"
                                label="Years of Experience"
                                name="yearsOfExperience"
                                fullWidth
                                variant="outlined"
                                helperText={<ErrorMessage name="yearsOfExperience" />}
                              />
                            )}
                            {index === 1 && (
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
                            {index === 2 && (
                              <Field
                                as={TextField}
                                margin="dense"
                                label="Vendor"
                                name="vender"
                                select
                                fullWidth
                                variant="outlined"
                                helperText={<ErrorMessage name="vender" />}
                                onChange={(event) => {
                                  const vendor = event.target.value;
                                  setFieldValue('vender', vendor);
                                  setFieldValue('model', '');
                                  fetchModels(vendor);
                                }}
                              >
                                {vendors.map((vendor) => (
                                    <MenuItem key={vendor} value={vendor}>
                                    {vendor}
                                    </MenuItem>
                                ))}
                              </Field>
                            )}
                            {index === 3 && (
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
                                {index === steps.length - 1 ? 'Finish' : 'Continue'}
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
                    <Typography>All steps completed - save changes?</Typography>
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