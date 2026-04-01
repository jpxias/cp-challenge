import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import * as React from "react";

const TopBar = () => {
  return (
    <React.Fragment>
      <Box
        sx={{
          display: "flex",
          alignItems: "center",
          textAlign: "center",
          justifyContent: "space-between",
          width: "100%",
          height: 50,
          boxShadow: "0 4px 6px rgba(0, 0, 0, 0.1)",
        }}
      >
        <Typography variant="h5" marginLeft={2}>
          Event Manager
        </Typography>
      </Box>
    </React.Fragment>
  );
};

export default TopBar;
