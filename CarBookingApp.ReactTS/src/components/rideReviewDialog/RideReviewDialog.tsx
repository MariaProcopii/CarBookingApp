import { useState } from 'react';
import { Dialog, DialogTitle, DialogContent, Button, TextField, Rating, Box, IconButton, Typography } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import * as Yup from "yup";
import { Form, Formik, Field, ErrorMessage, FormikHelpers } from "formik";
import OwnerDetails from '../ownerDetails/OwnerDetails';
import { TOwner } from '../../models/Owner';
import TipDialog from '../tipDialog/TipDialog';
import useAuth from '../../context/auth/UseAuth';
import { useTokenDecoder } from '../../utils/TokenUtils';
import ReviewService from '../../services/reviewService/ReviewService';
import useAPI from '../../context/api/UseAPI';
import { CreateReviewParams } from '../../services/reviewService/types';
import CustomSnackbar from '../customSnackbar/CustomSnackbar';

interface Props {
  open: boolean;
  handleClose: () => void;
  owner: TOwner
}

export default function RideReviewDialog({ open, handleClose, owner}: Props) {

    const dialogTitleStyle = {
        display: "flex",
        bgcolor: "primary.main",
        color: "primary.contrastText",
        justifyContent: "space-between",
        alignItems: "center"
    };

    const [openTipDialog, setOpenTipDialog] = useState(false);
    const [rating, setRating] = useState(1);
    const [snackbar, setSnackbar] = useState({ open: false, message: "somethig for tests", severity: "success" });

    const { token } = useAuth();
    const claims = useTokenDecoder(token ? token : "");

    const { instance } = useAPI();
    const { createReview } = ReviewService(instance);

    const initialValues = {
        reviewComment: ""
    };

    const validationSchema = Yup.object().shape({
        reviewComment: Yup.string()
          .required("Review comment is required.")
          .max(200, "Review comment can't exceed 200 characters.")
    });


    const onClose = () => {
      setOpenTipDialog(true);
      handleClose();
    };

    const handleCloseSnackbar = () => {
      setSnackbar({ ...snackbar, open: false });
    };

    const handleSubmit = async (values: any, formikHelpers: FormikHelpers<any>) => {

        const createReviewProps: CreateReviewParams = {
          rideReviewerId: claims.nameidentifier as number,
          rideRevieweeId: owner.id as number,
          rating: rating,
          reviewComment: values.reviewComment,
        }  
          console.log(createReviewProps);
        await createReview(createReviewProps, setSnackbar);

        formikHelpers.resetForm();
        setTimeout(() => {
          handleCloseSnackbar();
          onClose();
        }, 1000);
      }

  return (
    <>
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle sx={dialogTitleStyle}>Leave a review about driver</DialogTitle>
        <IconButton
            onClick={onClose}
            sx={{ position: 'absolute', right: 8, top: 8, color: "primary.contrastText" }}
        >
          <CloseIcon />
        </IconButton>
        <DialogContent>
            <OwnerDetails owner={owner} />
            <Formik initialValues={initialValues} onSubmit={handleSubmit} validationSchema={validationSchema}>
                {(props) => (
                <Form>
                    <Box display="flex" flexDirection="column" justifyContent="center" alignItems="center" gap={2}>
                    <Typography variant="body1" component="span" fontWeight="bold" >Driver Rating:</Typography>
                        <Rating
                            name="rating"
                            size="large"
                            precision={0.5}
                            value={rating}
                            onChange={(_, newValue) => {
                              setRating(newValue ? newValue : 1);
                            }}
                        />
                        <Field as={TextField}
                            label="Review Comment"
                            name="reviewComment"
                            placeholder="Live a review"
                            multiline
                            rows={4}
                            fullWidth
                            helperText={<ErrorMessage name="reviewComment" />}
                        />
                    </Box>
                    <Button 
                        type="submit"
                        color="primary"
                        variant="contained"
                        fullWidth
                        disabled={props.isSubmitting}
                    >
                        Submit
                    </Button>
                </Form>
                )}
            </Formik>
        </DialogContent>
    </Dialog>
    <TipDialog
        open={openTipDialog}
        onClose={() => setOpenTipDialog(false)}
        driverEmail={owner.email}
        tipperEmail={claims.emailaddress as string}
    />
    <CustomSnackbar 
        open={snackbar.open} 
        message={snackbar.message} 
        severity={snackbar.severity} 
        onClose={handleCloseSnackbar}
    />
    </>
  );
};