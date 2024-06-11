import React from 'react';
import { Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, TextField,
         Grid, FormControl, InputLabel, Select, MenuItem, Button } from '@mui/material';

export default function EditUserDialog({ open, setOpen, userInfo, setUserInfo, handleSave }) {
  const handleClose = () => {
    setOpen(false);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserInfo({
      ...userInfo,
      [name]: value,
    });
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="xs" fullWidth>
      <DialogTitle>Edit User Info</DialogTitle>
      <DialogContent>
        <DialogContentText>
          Update the user information below:
        </DialogContentText>
        <Formik
          initialValues={userInfo}
          validationSchema={userValidationSchema}
          onSubmit={(values, { setSubmitting }) => {
            handleSave(values);
            setSubmitting(false);
            handleClose();
          }}
        >
          {({ isSubmitting }) => (
            <Form>
              <Grid container spacing={2} sx={{ mt: 1 }}>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="First Name"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="firstName"
                    helperText={<ErrorMessage name="firstName" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Last Name"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="lastName"
                    helperText={<ErrorMessage name="lastName" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Email"
                    type="email"
                    fullWidth
                    variant="outlined"
                    name="email"
                    helperText={<ErrorMessage name="email" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Phone Number"
                    type="text"
                    fullWidth
                    variant="outlined"
                    name="phoneNumber"
                    helperText={<ErrorMessage name="phoneNumber" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="Years of Experience"
                    type="number"
                    fullWidth
                    variant="outlined"
                    name="yearsOfExperience"
                    helperText={<ErrorMessage name="yearsOfExperience" />}
                  />
                </Grid>
                <Grid item xs={12}>
                  <FormControl fullWidth sx={{ marginTop: 1 }}>
                    <InputLabel id="gender-select-label">Gender</InputLabel>
                    <Field
                      as={Select}
                      labelId="gender-select-label"
                      id="gender-select"
                      label="Gender"
                      name="gender"
                    >
                      <MenuItem value={"FEMALE"}>Female</MenuItem>
                      <MenuItem value={"MALE"}>Male</MenuItem>
                      <MenuItem value={"OTHER"}>Other</MenuItem>
                    </Field>
                  </FormControl>
                </Grid>
                <Grid item xs={12}>
                  <Field
                    as={TextField}
                    margin="dense"
                    label="New Password"
                    type="password"
                    fullWidth
                    variant="outlined"
                    name="password"
                    helperText={<ErrorMessage name="password" />}
                  />
                </Grid>
              </Grid>
              <DialogActions>
                <Button onClick={handleClose} color="secondary">Cancel</Button>
                <Button type="submit" color="primary" disabled={isSubmitting}>Save</Button>
              </DialogActions>
            </Form>
          )}
        </Formik>
      </DialogContent>
    </Dialog>
  );
}