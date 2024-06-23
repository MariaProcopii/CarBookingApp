import { AxiosInstance } from 'axios';
import { CreatePaymentParams, ExecutePaymentParams, SnackbarParams } from './types';
import { useNavigate } from 'react-router-dom';

function PaymentService(instance: AxiosInstance | null) {

    const navigate = useNavigate();

    async function createPayment(
        createPaymentParams: CreatePaymentParams
    ): Promise<void> {
        try {
            var returnUrl = "http://192.168.0.9:5173/payment/execute";
            var cancelUrl = "http://192.168.0.9:5173/";
            
            var CreatePaymentParamsWithURL = {
                ...createPaymentParams,
                returnUrl,
                cancelUrl
            }
            const response = await instance?.post("/payment/create", CreatePaymentParamsWithURL);
            window.location.replace(response?.data.approvalUrl);
          } catch (error) {
            console.error('Error creating payment:', error);
          }
    }

    async function executePayment(
        executePaymentParams: ExecutePaymentParams
    ): Promise<void> {
        try {
            await instance?.post("/payment/execute", executePaymentParams);
            navigate('/', { 
                replace: true,
                state: { open: true, message: "Payment handeled successfully!", severity: "success" } as SnackbarParams
            });
          } catch (error) {
            console.error('Error executing payment:', error);
            navigate('/', { 
                replace: true,
                state: { open: true, message: "Error while executing payment!", severity: "error" } as SnackbarParams
            });
          }
    }

    return {
        createPayment,
        executePayment
    };
}

export default PaymentService;