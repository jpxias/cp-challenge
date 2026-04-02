import { render, screen } from "@testing-library/react";
import { describe, test, expect, vi } from "vitest";
import userEvent from "@testing-library/user-event";
import EventListItem from "./EventListItem.component";
import { Event } from "../../models";
import "@testing-library/jest-dom/vitest";

describe("EventListItem Component", () => {
  const mockEvent: Event = {
    id: "123",
    title: "Title",
    description: "Description",
    startDate: "2026-04-01T10:00:00Z",
    endDate: "2026-04-01T12:00:00Z",
  };

  const mockOnClick = vi.fn();

  test("renders event details correctly", () => {
    render(<EventListItem event={mockEvent} onClick={mockOnClick} />);

    expect(screen.getByText("Title")).toBeInTheDocument();
    expect(screen.getByText("Description")).toBeInTheDocument();

    const startContainer = screen.getByText(/Start:/i);
    const endContainer = screen.getByText(/End:/i);

    expect(startContainer).toHaveTextContent(/04\/01\/2026/);
    expect(endContainer).toHaveTextContent(/04\/01\/2026/);
  });

  test("calls onClick with the event object when the list item is clicked", async () => {
    const user = userEvent.setup();
    render(<EventListItem event={mockEvent} onClick={mockOnClick} />);

    const clickableArea = screen.getByRole("button");
    await user.click(clickableArea);

    expect(mockOnClick).toHaveBeenCalledTimes(1);
    expect(mockOnClick).toHaveBeenCalledWith(mockEvent);
  });
});
