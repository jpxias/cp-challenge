import { render, screen, waitFor } from "@testing-library/react";
import { describe, test, expect, vi, beforeEach } from "vitest";
import userEvent from "@testing-library/user-event";
import EventManagerScreen from "./EventManager.screen";
import { useGetEvents } from "../../api/get-events-endpoint/get-events-endpoint";
import "@testing-library/jest-dom/vitest";

vi.mock("../../api/get-events-endpoint/get-events-endpoint", () => ({
  useGetEvents: vi.fn(),
}));

vi.mock("../../components/CreateEventModal/CreateEventModal.component", () => ({
  default: ({ open, handleClose, handleSubmit }: any) =>
    open ? (
      <div data-testid="event-modal">
        <button onClick={handleClose}>Close</button>
        <button onClick={handleSubmit}>Submit</button>
      </div>
    ) : null,
}));

vi.mock("../../components/EventListItem/EventListItem.component", () => ({
  default: ({ event, onClick }: any) => (
    <div data-testid="event-item" onClick={() => onClick(event)}>
      {event.title}
    </div>
  ),
}));

describe("EventManagerScreen", () => {
  const mockRefetch = vi.fn();
  const mockEvents = [
    { id: "1", title: "Meeting A", description: "First Description" },
    { id: "2", title: "Workshop B", description: "Second Description" },
  ];

  beforeEach(() => {
    vi.clearAllMocks();
    (useGetEvents as any).mockReturnValue({
      data: { data: { data: { items: mockEvents } } },
      refetch: mockRefetch,
    });
  });

  test("renders the list of events correctly", () => {
    render(<EventManagerScreen />);

    const items = screen.getAllByTestId("event-item");
    expect(items).toHaveLength(2);
    expect(screen.getByText("Meeting A")).toBeVisible();
    expect(screen.getByText("Workshop B")).toBeVisible();
  });

  test("opens the modal when 'New Event' is clicked", async () => {
    const user = userEvent.setup();
    render(<EventManagerScreen />);

    const newEventBtn = screen.getByRole("button", { name: /New event/i });
    await user.click(newEventBtn);

    expect(screen.getByTestId("event-modal")).toBeVisible();
  });

  test("triggers a filtered API call when searching (with debounce)", async () => {
    const user = userEvent.setup();
    render(<EventManagerScreen />);

    const searchInput = screen.getByRole("textbox");
    await user.type(searchInput, "Meeting");

    // Wait the 500ms debounce
    await waitFor(
      () => {
        expect(useGetEvents).toHaveBeenCalledWith(
          expect.objectContaining({
            $filter:
              "contains(title, 'Meeting') or contains(description, 'Meeting')",
          }),
        );
      },
      { timeout: 1000 },
    );
  });

  test("refetches data and closes modal on successful submit", async () => {
    const user = userEvent.setup();
    render(<EventManagerScreen />);

    await user.click(screen.getByRole("button", { name: /New event/i }));

    const submitBtn = screen.getByRole("button", { name: /Submit/i });
    await user.click(submitBtn);

    expect(mockRefetch).toHaveBeenCalled();
    expect(screen.queryByTestId("event-modal")).not.toBeInTheDocument();
  });

  test("opens modal in view-only mode when an item is clicked", async () => {
    const user = userEvent.setup();
    render(<EventManagerScreen />);

    const firstItem = screen.getByText("Meeting A");
    await user.click(firstItem);

    expect(screen.getByTestId("event-modal")).toBeVisible();
  });
});
