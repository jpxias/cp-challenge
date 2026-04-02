import { render, screen, waitFor } from "@testing-library/react";
import { describe, test, expect, beforeEach, vi } from "vitest";
import userEvent from "@testing-library/user-event";
import CreateEventModal from "./CreateEventModal.component";
import { postEvents } from "../../api/create-event-endpoint/create-event-endpoint";
import "@testing-library/jest-dom/vitest";

vi.mock("../../api/create-event-endpoint/create-event-endpoint", () => ({
  postEvents: vi.fn(),
}));

const mockedPostEvents = postEvents as any;

const mockProps = {
  open: true,
  handleClose: vi.fn(),
  handleSubmit: vi.fn(),
  event: null,
  viewOnly: false,
};

describe("CreateEventModal", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  test("shows validation error for empty required field", async () => {
    const user = userEvent.setup();
    render(<CreateEventModal {...mockProps} />);

    const input = screen.getByLabelText(/Event Title/i);
    await user.click(input);

    const submitBtn = screen.getByRole("button", { name: /Submit/i });
    await user.click(submitBtn);

    expect(await screen.findByText(/Event title is required/i)).toBeVisible();
  });

  test("validates that End Date cannot be before Start Date", async () => {
    const user = userEvent.setup();
    render(<CreateEventModal {...mockProps} />);

    const startDateInput = screen.getByLabelText(/Start Date/i);
    const endDateInput = screen.getByLabelText(/End Date/i);

    await user.type(startDateInput, "2026-04-01T15:00");
    await user.type(endDateInput, "2026-04-01T10:00");

    const submitBtn = screen.getByRole("button", { name: /Submit/i });
    await user.click(submitBtn);

    expect(
      await screen.findByText(/End date must be after start date/i),
    ).toBeVisible();
  });

  test("enforces the 50 character limit on the title", async () => {
    const user = userEvent.setup();
    render(<CreateEventModal {...mockProps} />);

    const input = screen.getByLabelText(/Event Title/i);
    const longTitle = "a".repeat(51);

    await user.clear(input);
    await user.type(input, longTitle);

    const submitBtn = screen.getByRole("button", { name: /Submit/i });
    await user.click(submitBtn);

    const errorMessage = await screen.findByText(
      /title must be at most 50 characters/i,
    );
    expect(errorMessage).toBeVisible();
  });

  test("successfully submits when all fields are valid", async () => {
    const user = userEvent.setup();
    mockedPostEvents.mockResolvedValueOnce({
      data: { data: { id: "99", title: "Success" } },
    });

    render(<CreateEventModal {...mockProps} />);

    const titleInput = screen.getByLabelText(/Event Title/i);
    const descriptionInput = screen.getByLabelText(/Description/i);
    const startDateInput = screen.getByLabelText(/Start Date/i);
    const endDateInput = screen.getByLabelText(/End Date/i);

    await user.type(titleInput, "Success Event");
    await user.type(descriptionInput, "Description success");
    await user.type(startDateInput, "2026-04-01T10:00");
    await user.type(endDateInput, "2026-04-01T11:00");

    const submitBtn = screen.getByRole("button", { name: /Submit/i });
    await user.click(submitBtn);

    await waitFor(() => {
      expect(mockedPostEvents).toHaveBeenCalledWith(
        expect.objectContaining({
          title: "Success Event",
          description: "Description success",
          startDate: new Date("2026-04-01T10:00").toISOString(),
          endDate: new Date("2026-04-01T11:00").toISOString(),
        }),
      );
      expect(mockProps.handleSubmit).toHaveBeenCalled();
    });
  });
});
