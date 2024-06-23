export interface CreatePaymentParams {
    amount: number;
    tipperEmail: string;
    driverEmail: string;
}

export interface ExecutePaymentParams {
    paymentId: string | null,
    payerId: string | null
}

export interface SnackbarParams {
    open: boolean; 
    message: string; 
    severity: string;
}