import * as Yup from "yup";

export const eventInitialValues = {
  id: "",
  title: "",
  description: "",
  startDate: "",
  endDate: "",
};

export const eventValidationSchema = Yup.object({
  title: Yup.string().required("Event name is required").max(50),
  description: Yup.string().notRequired().max(300),
  startDate: Yup.date().required("Start date is required"),
  endDate: Yup.date()
    .required("End date is required")
    .min(Yup.ref("startDate"), "End date must be after start date"),
});
