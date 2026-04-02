import SearchIcon from "@mui/icons-material/Search";
import { Button, List, TextField } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import CreateEventModal from "../../components/CreateEventModal/CreateEventModal.component";
import EventListItem from "../../components/EventListItem/EventListItem.component";
import TopBar from "../../components/TopBar/TopBar";
import { Event } from "../../models";
import { useGetEvents } from "../../api/get-events-endpoint/get-events-endpoint";
import debounce from "lodash/debounce";
import LoadingSkeleton from "../../components/LoadingSkeleton/LoadingSekeleton.component";
import { useToast } from "../../providers/toastProvider";

const EventManagerScreen = () => {
  const [modalOpen, setModalOpen] = useState<boolean>(false);
  const [selectedEvent, setSelectedEvent] = useState<Event | null>(null);
  const [viewOnly, setViewOnly] = useState<boolean>(false);
  const [filter, setFilter] = useState<string>();
  const { showError } = useToast();
  const { data, refetch, isLoading } = useGetEvents({
    $filter: filter
      ? `contains(title, '${filter}') or contains(description, '${filter}')`
      : undefined,
  });

  const events = data?.data.data?.items;
  const addEvent = (event: Event | null) => {
    setViewOnly(false);
    setSelectedEvent(event);
    setModalOpen(true);
  };

  const viewEvent = (event: Event) => {
    setViewOnly(true);
    setSelectedEvent(event);
    setModalOpen(true);
  };

  const debouncedSetFilter = useMemo(
    () =>
      debounce((value: string) => {
        setFilter(value);
      }, 500),
    [],
  );

  useEffect(() => {
    return () => {
      debouncedSetFilter.cancel();
    };
  }, [debouncedSetFilter]);

  return (
    <>
      <CreateEventModal
        open={modalOpen}
        event={selectedEvent}
        handleClose={() => setModalOpen(false)}
        handleSubmit={(data) => {
          if (data?.isSuccess) {
            setModalOpen(false);
            refetch();
          } else {
            showError(data?.error ?? "Something went wrong");
          }
        }}
        viewOnly={viewOnly}
      />

      <div className="event-container">
        <TopBar />
        <div
          style={{
            display: "flex",
            width: "100%",
            justifyContent: "flex-end",
            alignItems: "center",
          }}
        >
          <TextField
            fullWidth
            onChange={(e) => debouncedSetFilter(e.target.value)}
            style={{ padding: 10 }}
            slotProps={{
              input: {
                endAdornment: <SearchIcon />,
              },
            }}
          />
          <Button
            variant="contained"
            onClick={() => addEvent(null)}
            style={{ margin: 20 }}
          >
            New event
          </Button>
        </div>

        {isLoading ? (
          <LoadingSkeleton />
        ) : (
          <List sx={{ width: "100%", bgcolor: "background.paper" }}>
            {events?.map((event) => (
              <EventListItem key={event.id} event={event} onClick={viewEvent} />
            ))}
          </List>
        )}
      </div>
    </>
  );
};

export default EventManagerScreen;
