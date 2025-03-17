import { Toaster } from "sonner";

const ToastProvider = () => {
    return (
        <Toaster
            richColors
            style={{
                fontSize: "2rem",
                fontFamily: "'Poppins', sans-serif"
            }}
            position="bottom-right"
            duration={4000}
            closeButton
            expand={true}
            visibleToasts={3}
        />
    );
};

export default ToastProvider;
