import React, { createContext, useContext, useState } from "react";
import { Snackbar, Alert } from "@mui/material";

interface ToastContextType {
  showError: (message: string) => void;
}

const ToastContext = createContext<ToastContextType | undefined>(undefined);

export const ToastProvider = ({ children }: { children: React.ReactNode }) => {
  const [open, setOpen] = useState(false);
  const [message, setMessage] = useState("");

  const showError = (msg: string) => {
    setMessage(msg);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  return (
    <ToastContext.Provider value={{ showError }}>
      {children}
      <Snackbar
        open={open}
        autoHideDuration={6000}
        onClose={handleClose}
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }} // Good for UX
      >
        <Alert severity="error" variant="filled" onClose={handleClose}>
          {message}
        </Alert>
      </Snackbar>
    </ToastContext.Provider>
  );
};

export const useToast = () => {
  const context = useContext(ToastContext);
  if (!context) {
    throw new Error("useToast must be used within a ToastProvider");
  }
  return context;
};
