import { Snackbar, Alert } from '@mui/material';
import Slide from '@mui/material/Slide';

export default function CustomSnackbar({ open, message, severity, onClose }) {

function SlideTransition(props) {
  return <Slide {...props} direction="up" />;
}

const alertStyles = {
    backgroundColor: severity === 'success' ? '#569c79' : '#a41116',
    color: '#fff',
};

    return (
        <Snackbar 
            open={open}
            autoHideDuration={6000} 
            onClose={onClose}
            TransitionComponent={SlideTransition}
            anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        >
            <Alert 
                onClose={onClose}
                severity={severity}
                sx={{ ...alertStyles, width: '100%' }}
            >
                {message}
            </Alert>
        </Snackbar>
    );
};
