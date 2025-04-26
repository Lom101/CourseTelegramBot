// routes/routes.js
import BlockPage from "../pages/BlockPage";
import UsersPage from "../pages/UsersPage";
import LoginPage from "../pages/LoginPage";
import ContentItemPage from "../pages/ContentItemPage";
import TopicsPage from "../pages/TopicsPage";

// Защищённые маршруты (для авторизованных пользователей)
export const authRoutes = [
    { path: "/users", Component: UsersPage },
    { path: "/blocks", Component: BlockPage },
    { path: "/blocks/:blockId", Component: TopicsPage },
    { path: "/topic/:topicId", Component: ContentItemPage }
];

// Публичные маршруты (для всех)
export const publicRoutes = [
    { path: "/login", Component: LoginPage }
];
