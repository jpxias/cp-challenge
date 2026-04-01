import { Route, Routes } from "react-router-dom";
import EventManagerScreen from "../screens/eventManager/EventManager.screen";
import NotFoundScreen from "../screens/notFound/NotFound.screen";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<EventManagerScreen />} />
      <Route path="*" element={<NotFoundScreen />} />
    </Routes>
  );
};

export default AppRoutes;
