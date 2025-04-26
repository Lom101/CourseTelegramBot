import { Link, useLocation, useNavigate } from "react-router-dom";
import { useContext } from "react";
import { AuthContext } from "./AuthContext";

export default function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();
  const { logout } = useContext(AuthContext);

  const menuItems = [
    { label: "Участники", to: "/users" },
    { label: "Материалы", to: "/blocks" },
    { label: "Тесты", to: "/tests" },
  ];

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <nav className="relative flex items-center justify-center pt-6 mb-8 px-6">
      {/* Центр — меню */}
      <ul className="flex gap-6 px-6 py-2 rounded-full shadow-md border border-yellow-400 bg-yellow-300">
        {menuItems.map(({ label, to }) => {
          const isActive = location.pathname === to;
          return (
            <li key={to}>
              <Link
                to={to}
                className={`flex items-center gap-1 px-4 py-1 rounded-full text-sm font-medium transition-colors duration-200
                  ${
                    isActive
                      ? "bg-yellow-400 text-black"
                      : "text-black hover:bg-yellow-400"
                  }`}
              >
                {label}
              </Link>
            </li>
          );
        })}
      </ul>

      {/* Справа — кнопка "Выйти" */}
      <button
        onClick={handleLogout}
        className="absolute right-6 top-6 px-4 py-2 rounded-full text-sm font-medium transition-colors duration-200 text-black bg-yellow-300 border border-yellow-400 shadow-md hover:bg-yellow-400"
      >
        Выйти
      </button>
    </nav>
  );
}
