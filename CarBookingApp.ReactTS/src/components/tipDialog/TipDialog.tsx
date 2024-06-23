import { Dialog, DialogTitle, DialogContent, Button, TextField, IconButton } from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import * as Yup from "yup";
import { Form, Formik, Field, ErrorMessage, FormikHelpers } from "formik";
import useAPI from '../../context/api/UseAPI';
import PaymentService from '../../services/paymentService/PaymentService';

interface Props {
  open: boolean;
  onClose: () => void;
  driverEmail: string;
  tipperEmail: string;
}

export default function TipDialog({ open, onClose, driverEmail, tipperEmail }: Props) {

    const { instance } = useAPI();
    const { createPayment } = PaymentService(instance);

    const initialValues = {
        amount: 1
    };

    const validationSchema = Yup.object().shape({
        amount: Yup.number()
            .required("Amount is required")
            .min(1.00, "At least 1 lei is required.")
    });

    const handleSubmit = async (values: any, formikHelpers: FormikHelpers<any>) => {
        const createPaymentProps = {
            amount: values.amount,
            tipperEmail: tipperEmail,
            driverEmail: driverEmail
        };

        await createPayment(createPaymentProps);

        setTimeout(() => {
            formikHelpers.resetForm();
            formikHelpers.setSubmitting(false);
        }, 5000);
    };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
        <DialogTitle>
            Leave a Tip for the Driver
            <IconButton
                onClick={onClose}
                sx={{ position: 'absolute', right: 8, top: 8, color: (theme) => theme.palette.grey[500] }}
            >
                <CloseIcon />
            </IconButton>
        </DialogTitle>
        <DialogContent>
            <Formik initialValues={initialValues} onSubmit={handleSubmit} validationSchema={validationSchema}>
                {(props) => (
                    <Form>
                        <Field as={TextField}
                            autoFocus
                            required
                            margin="dense"
                            label="Tip Amount (USD)"
                            placeholder="Enter amount"
                            type="number"
                            fullWidth
                            variant="outlined"
                            name="amount"
                            helperText={<ErrorMessage name="amount" />}
                        />
                        <Button
                            type="submit"
                            color="primary"
                            variant="contained"
                            fullWidth
                            disabled={props.isSubmitting}
                        >
                            {props.isSubmitting ? "Loading" : "Pay via PayPal"}
                        </Button>
                    </Form>
                )}
            </Formik>
        </DialogContent>
    </Dialog>
  );
};