import {
  Box,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
} from "@mui/material";
import { format } from "date-fns";
import { Event } from "../../models";

interface IEventListProps {
  event: Event;
  onClick: (event: Event) => void;
}

const EventList = ({ event, onClick }: IEventListProps) => {
  const startDate = event.startDate
    ? format(new Date(event.startDate), "Pp")
    : "";
  const endDate = event.endDate ? format(new Date(event.endDate), "Pp") : "";

  return (
    <ListItem disablePadding divider style={{ width: `100%` }}>
      <ListItemButton onClick={() => onClick(event)}>
        <ListItemText primary={event.title} secondary={event.description} />
        <Box sx={{ textAlign: "right", ml: 2, minWidth: "fit-content" }}>
          <Typography variant="caption" color="text.secondary" display="block">
            Start: {startDate}
          </Typography>
          <Typography variant="caption" color="text.secondary" display="block">
            End: {endDate}
          </Typography>
        </Box>
      </ListItemButton>
    </ListItem>
  );
};

export default EventList;
