import CloseIcon from "@mui/icons-material/Close";
import { Box, Button, IconButton, Modal, TextField } from "@mui/material";
import { format } from "date-fns";
import { useFormik } from "formik";
import { useEffect } from "react";
import "./CreateEventModal.component.css";
import {
  eventInitialValues,
  eventValidationSchema,
} from "./CreateEventModal.schema";
import { Event } from "../../models";
import { postEvents } from "../../api/create-event-endpoint/create-event-endpoint";

interface ICreateEventModalProps {
  open: boolean;
  handleClose: () => void;
  handleSubmit: (event: Event | undefined) => void;
  event: Event | null;
  viewOnly: boolean;
}

const CreateEventModal = ({
  open,
  handleClose,
  event,
  handleSubmit,
  viewOnly,
}: ICreateEventModalProps) => {
  const formik = useFormik({
    initialValues: eventInitialValues,
    validationSchema: eventValidationSchema,
    onSubmit: async (values) => {
      const newEvent: Event = {
        title: values.title,
        description: values.description,
        startDate: new Date(values.startDate).toISOString(),
        endDate: new Date(values.endDate).toISOString(),
      };

      const { data } = await postEvents(newEvent);
      if (data.data) handleSubmit(data.data);
    },
  });

  const formatDate = (date: string): string => {
    return format(new Date(date), "yyyy-MM-dd'T'HH:mm");
  };

  const defaultSlotProps = {
    htmlInput: {
      readOnly: viewOnly,
    },
  };

  useEffect(() => {
    formik.resetForm();

    if (event) {
      event = event as Event;

      formik.setValues({
        id: event.id ?? "",
        title: event.title ?? "",
        description: event.description ?? "",
        startDate: event.startDate ? formatDate(event.startDate) : "",
        endDate: event.endDate ? formatDate(event.endDate) : "",
      });
    } else {
      formik.setValues(eventInitialValues);
    }
  }, [event]);

  return (
    <Modal open={open} onClose={handleClose} aria-labelledby="Create new event">
      <Box
        component="form"
        style={{ backgroundColor: "white" }}
        onSubmit={formik.handleSubmit}
        className="create-event-modal"
        sx={{
          p: 4,
        }}
      >
        <IconButton
          edge="end"
          color="inherit"
          onClick={handleClose}
          aria-label="close"
          className="close-modal-icon"
        >
          <CloseIcon />
        </IconButton>
        <div className="create-event-form-container">
          <div className="create-event-form">
            <TextField
              label="Event Title"
              name="title"
              value={formik.values.title}
              required
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.title && Boolean(formik.errors.title)}
              helperText={formik.touched.title && formik.errors.title}
              fullWidth
              slotProps={defaultSlotProps}
            />
            <TextField
              label="Description"
              name="description"
              multiline
              rows={3}
              value={formik.values.description}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={
                formik.touched.description && Boolean(formik.errors.description)
              }
              helperText={
                formik.touched.description && formik.errors.description
              }
              fullWidth
              slotProps={defaultSlotProps}
            />
            <TextField
              label="Start Date"
              name="startDate"
              type="datetime-local"
              value={formik.values.startDate}
              required
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={
                formik.touched.startDate && Boolean(formik.errors.startDate)
              }
              helperText={formik.touched.startDate && formik.errors.startDate}
              fullWidth
              slotProps={{ ...defaultSlotProps, inputLabel: { shrink: true } }}
            />
            <TextField
              label="End Date"
              name="endDate"
              type="datetime-local"
              value={formik.values.endDate}
              required
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              error={formik.touched.endDate && Boolean(formik.errors.endDate)}
              helperText={formik.touched && formik.errors.endDate}
              fullWidth
              slotProps={{ ...defaultSlotProps, inputLabel: { shrink: true } }}
            />

            {!viewOnly && (
              <Button type="submit" variant="contained" color="primary">
                Submit
              </Button>
            )}
          </div>
        </div>
      </Box>
    </Modal>
  );
};

export default CreateEventModal;
