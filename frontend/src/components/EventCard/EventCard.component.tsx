import MoreVertIcon from "@mui/icons-material/MoreVert";

import {
  Card,
  CardActionArea,
  CardContent,
  CardHeader,
  IconButton,
  Menu,
  MenuItem,
  Typography,
} from "@mui/material";
import { format } from "date-fns";
import { useState } from "react";
import "./EventCard.component.css";
import { Event } from "../../models";

interface IEventCardProps {
  event: Event;
  onClickCard: (event: Event) => void;
  onClickEdit: (event: Event) => void;
  onClickDelete: (event: Event) => void;
}

const EventCard = ({
  event,
  onClickCard,
  onClickEdit,
  onClickDelete,
}: IEventCardProps) => {
  const startDate = event.startDate
    ? format(new Date(event.startDate), "Pp")
    : "";
  const endDate = event.endDate ? format(new Date(event.endDate), "Pp") : "";
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    event.preventDefault();
    event.stopPropagation();
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleEdit = (e: React.MouseEvent<HTMLLIElement, MouseEvent>) => {
    e.preventDefault();
    e.stopPropagation();
    onClickEdit(event);
    handleClose();
  };

  const handleDelete = (e: React.MouseEvent<HTMLLIElement, MouseEvent>) => {
    e.preventDefault();
    e.stopPropagation();
    onClickDelete(event);
    handleClose();
  };

  return (
    <Card className="event-card">
      <CardActionArea onClick={() => onClickCard(event)}>
        <CardHeader
          action={
            <>
              <IconButton
                aria-label="more"
                id="long-button"
                aria-controls={open ? "long-menu" : undefined}
                aria-expanded={open ? "true" : undefined}
                aria-haspopup="true"
                onClick={handleClick}
              >
                <MoreVertIcon />
              </IconButton>
              <Menu
                id="long-menu"
                MenuListProps={{
                  "aria-labelledby": "long-button",
                }}
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
              >
                <MenuItem onClick={handleEdit}>Edit</MenuItem>

                <MenuItem onClick={handleDelete}>Delete</MenuItem>
              </Menu>
            </>
          }
          title={event.title}
        />
        <CardContent>
          <Typography color="gray">{event.description}</Typography>
          <div style={{ marginTop: 50, display: "flex", gap: 10 }}>
            <Typography variant="body2">Start: {startDate}</Typography>
            <Typography variant="body2">End: {endDate}</Typography>
          </div>
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default EventCard;
