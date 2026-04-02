import React, { createContext, useContext, useState } from "react";
import { Snackbar, Alert } from "@mui/material";

const ToastContext = createContext({
  showError: (message: string) => {},
});

export const ToastProvider = ({ children }: { children: React.ReactNode }) => {
  const [open, setOpen] = useState(false);
  const [message, setMessage] = useState("");

  const showError = (msg: string) => {
    setMessage(msg);
    setOpen(true);
  };

  return (
    <ToastContext.Provider value={{ showError }}>
      <Snackbar
        open={open}
        autoHideDuration={6000}
        onClose={() => setOpen(false)}
      >
        <Alert severity="error" variant="filled" onClose={() => setOpen(false)}>
          {message}
        </Alert>
      </Snackbar>
      {children}
    </ToastContext.Provider>
  );
};

export const useToast = () => useContext(ToastContext);
