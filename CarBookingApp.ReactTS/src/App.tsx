import ThemeProvider from "./context/theme/ThemeProvider";
import AuthProvider from "./context/auth/AuthProvider";
import APIProvider from "./context/api/APIProvider";
import ErrorProvider from "./context/error/ErrorProvider";
import AppRoutes from "./routes/AppRoutes";
import { BrowserRouter } from "react-router-dom";

function App() {
    return (
        <ThemeProvider>
            <AuthProvider>
                <ErrorProvider>
                    <BrowserRouter>
                        <APIProvider>
                            <AppRoutes />
                        </APIProvider>
                    </BrowserRouter>
                </ErrorProvider>
            </AuthProvider>
        </ThemeProvider>
    );
}

export default App
