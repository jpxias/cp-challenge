import { SnackbarProvider } from "notistack";
import { BrowserRouter } from "react-router-dom";
import "./App.css";
import AppRoutes from "./routes/AppRoutes";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

function App() {
  const queryClient = new QueryClient();

  return (
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <SnackbarProvider>
          <AppRoutes />
        </SnackbarProvider>
      </QueryClientProvider>
    </BrowserRouter>
  );
}

export default App;
