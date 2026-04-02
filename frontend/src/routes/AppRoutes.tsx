import { Route, Routes } from "react-router-dom";
import EventManagerScreen from "../screens/EventManager/EventManager.screen";
import NotFoundScreen from "../screens/NotFound/NotFound.screen";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<EventManagerScreen />} />
      <Route path="*" element={<NotFoundScreen />} />
    </Routes>
  );
};

export default AppRoutes;
