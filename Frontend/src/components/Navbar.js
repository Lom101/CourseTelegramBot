import { Link, useLocation, useNavigate } from "react-router-dom";
import { useContext, useState, useEffect, useRef } from "react";
import { AuthContext } from "./AuthContext";

export default function Navbar() {
  const location = useLocation();
  const navigate = useNavigate();
  const { logout } = useContext(AuthContext);

  // Изначально меню свернуто
  const [isMenuVisible, setIsMenuVisible] = useState(false);

  // Реф для меню и кнопки меню, чтобы отслеживать клики вне этих элементов
  const menuRef = useRef(null);
  const buttonRef = useRef(null);

  // Элементы меню
  const menuItems = [
    { label: "Участники", to: "/users" },
    { label: "Материалы", to: "/blocks" },
    { label: "Тесты", to: "/tests" },
  ];

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const toggleMenuVisibility = () => {
    setIsMenuVisible(prevState => !prevState); // Переключение видимости меню
  };

  const handleMenuItemClick = () => {
    setIsMenuVisible(false); // Закрытие меню после клика на элемент меню
  };

  // Закрытие меню при клике вне меню и кнопки
  useEffect(() => {
    const handleClickOutside = (event) => {
      // Проверяем, был ли клик вне меню и вне кнопки
      if (
        menuRef.current && !menuRef.current.contains(event.target) &&
        buttonRef.current && !buttonRef.current.contains(event.target)
      ) {
        setIsMenuVisible(false); // Закрытие меню при клике вне меню и кнопки
      }
    };

    document.addEventListener("click", handleClickOutside);

    return () => {
      document.removeEventListener("click", handleClickOutside);
    };
  }, []);

  return (
    <div>
      {/* Меню */}
      <nav
        ref={menuRef} // Привязка рефа для отслеживания кликов вне меню
        className={`fixed top-0 left-0 h-full w-64 bg-gray-800 text-white p-6 shadow-lg transform transition-all duration-300 ${isMenuVisible ? "translate-x-0 z-40" : "-translate-x-full z-40"}`}
      >
        {/* Заголовок меню */}
        <h2 className="text-2xl font-bold mb-8 text-gray-200">Меню</h2>

        {/* Меню */}
        <ul className="space-y-4">
          {menuItems.map(({ label, to }) => {
            const isActive = location.pathname === to;
            return (
              <li key={to}>
                <Link
                  to={to}
                  onClick={handleMenuItemClick} // При клике скрываем меню
                  className={`flex items-center gap-2 px-4 py-2 rounded-lg text-sm font-medium transition-colors duration-200
                    ${isActive
                      ? "bg-gray-600 text-gray-300"
                      : "text-gray-300 hover:bg-gray-700 hover:text-white"}`}
                >
                  {label}
                </Link>
              </li>
            );
          })}
        </ul>

        {/* Кнопка "Выйти" */}
        <button
          onClick={handleLogout}
          className="absolute bottom-6 left-6 px-4 py-2 rounded-lg text-sm font-medium text-gray-800 bg-gray-300 border border-gray-400 shadow-md hover:bg-gray-400"
        >
          Выйти
        </button>
      </nav>

      {/* Гамбургер-меню */}
      <button
        ref={buttonRef} // Привязка рефа к кнопке меню
        onClick={toggleMenuVisibility}
        className="fixed top-4 left-4 text-gray-800 p-4 bg-gray-300 rounded-full shadow-md z-10" // Кнопка сверху с отрицательным z-index
      >
        <span className="block w-6 h-0.5 bg-gray-800 mb-1"></span>
        <span className="block w-6 h-0.5 bg-gray-800 mb-1"></span>
        <span className="block w-6 h-0.5 bg-gray-800"></span>
      </button>
    </div>
  );
}
