import { useEffect } from 'react';
import {CircularProgress, Backdrop} from '@mui/material';
import useAPI from '../../context/api/UseAPI';
import PaymentService from '../../services/paymentService/PaymentService';


export function ExecutePayment() {
  const { instance } = useAPI();
  const { executePayment } = PaymentService(instance);

  useEffect( () => {

    const params = new URLSearchParams(window.location.search);
    const paymentId = params.get("paymentId");
    const payerId = params.get("PayerID");

    const executePaymentProps = {
        paymentId,
        payerId
    }

     executePayment(executePaymentProps);
  }, []);

  return (
    <Backdrop
        sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}
        open={true}
    >
        <CircularProgress color="inherit" />
  </Backdrop>
  );
};