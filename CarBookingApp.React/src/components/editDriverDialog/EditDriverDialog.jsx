import React, { useState, useEffect } from 'react';
import {
  Dialog, DialogActions, DialogContent, DialogTitle, Button, Stepper, Step, StepLabel, StepContent, Box,
  TextField, Typography, MenuItem, CircularProgress, Paper
} from '@mui/material';
import { Form, Formik, Field, ErrorMessage } from 'formik';
import * as Yup from 'yup';
import axios from 'axios';

const steps = ['Vehicle Details', 'Vendor', 'Model'];

export default function EditDriverDialog({ open, setOpen, vehicleDetail, handleSave }) {
  const [activeStep, setActiveStep] = useState(0);
  const [vendors, setVendors] = useState([]);
  const [models, setModels] = useState([]);
  const [loadingModels, setLoadingModels] = useState(false);

  const initialValues = {
    manufactureYear: vehicleDetail.manufactureYear,
    registrationNumber: vehicleDetail.registrationNumber,
    vender: vehicleDetail.vehicle.vender,
    model: vehicleDetail.vehicle.model
  };

  useEffect(() => {
    setTimeout(() => {
        fetchVendors();
      }, 10);
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
    setLoadingModels(true);
    axios.get(`http://192.168.0.9:5239/vehicle/pick/model?vendor=${vendorName}`)
      .then((response) => {
        setModels(response.data);
        setLoadingModels(false);
      })
      .catch((error) => {
        console.error('Error fetching models:', error);
      });
  };

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
    })
  ];

  const handleClose = () => {
    setOpen(false);
    setActiveStep(0);
  };

  const handleNext = (isValid) => {
    if (isValid) {
      setActiveStep((prevActiveStep) => prevActiveStep + 1);
    }
  };

  const handleBack = () => {
    setActiveStep((prevActiveStep) => prevActiveStep - 1);
  };

  const handleSubmit = (values) => {
    setActiveStep(0);
    const formatedValues = {
        manufactureYear: values.manufactureYear,
        registrationNumber: values.registrationNumber,
        vehicle: {
            vender: values.vender,
            model: values.model
        }
    };

    handleSave(formatedValues);
    handleClose();
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
          {({ isValid, values, setFieldValue }) => (
            <Form>
              <Box sx={{ maxWidth: 400 }}>
                <Stepper activeStep={activeStep} orientation="vertical">
                  {steps.map((label, index) => (
                    <Step key={label}>
                      <StepLabel>
                        {label}
                      </StepLabel>
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
                                {loadingModels ? (
                                  <MenuItem disabled>
                                    <CircularProgress size={24} />
                                  </MenuItem>
                                ) : (
                                  models.map((model) => (
                                    <MenuItem key={model} value={model}>
                                      {model}
                                    </MenuItem>
                                  ))
                                )}
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
                              {index === steps.length - 1 ? 'Finish' : 'Continue'}
                            </Button>
                          </DialogActions>
                        </Box>
                      </StepContent>
                    </Step>
                  ))}
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