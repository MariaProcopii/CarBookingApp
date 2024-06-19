import Slide from "@mui/material/Slide";
import { Snackbar, Alert } from "@mui/material";
import { SlideProps } from "@mui/material/Slide";

interface Props {
    open: any;
    message: any;
    severity: any;
    onClose: any;
}

export default function CustomSnackbar({ open, message, severity, onClose }: Props) {

    function SlideTransition(props: SlideProps) {
        return <Slide {...props} direction="up" />;
    }

    let backgroundColor;

    switch (severity) {
        case "success":
            backgroundColor = "#569c79";
            break;
        case "error":
            backgroundColor = "#a41116";
            break;
        case "warning":
            backgroundColor = "#d39b32";
            break;
        default:
            backgroundColor = "#000";
    }

    const alertStyles = {
        backgroundColor: backgroundColor,
        color: "#fff",
    };

    return (
        <Snackbar
            open={open}
            autoHideDuration={6000}
            onClose={onClose}
            TransitionComponent={SlideTransition}
            anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        >
            <Alert
                onClose={onClose}
                severity={severity}
                sx={{ ...alertStyles, width: "100%" }}
            >
                {message}
            </Alert>
        </Snackbar>
    );
}