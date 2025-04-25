import { Link, useLocation } from "react-router-dom";

export default function Navbar() {
  const location = useLocation();

  const menuItems = [
    { label: "Участники", to: "/users" },
    {
      label: "Материалы",
      to: "/blocks",
      icon: (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          width="18"
          height="18"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
          className="mr-1"
        >
          <path stroke="none" d="M0 0h24v24H0z" fill="none" />
          <path d="M6 17.6l-2 -1.1v-2.5" />
          <path d="M4 10v-2.5l2 -1.1" />
          <path d="M10 4.1l2 -1.1l2 1.1" />
          <path d="M18 6.4l2 1.1v2.5" />
          <path d="M20 14v2.5l-2 1.12" />
          <path d="M14 19.9l-2 1.1l-2 -1.1" />
          <path d="M12 12l2 -1.1" />
          <path d="M18 8.6l2 -1.1" />
          <path d="M12 12l0 2.5" />
          <path d="M12 18.5l0 2.5" />
          <path d="M12 12l-2 -1.12" />
          <path d="M6 8.6l-2 -1.1" />
        </svg>
      ),
    },
  ];

  return (
    <nav className="flex justify-center pt-6 mb-8">
      <ul className="flex gap-6 px-6 py-2 rounded-full shadow-md border border-yellow-400 bg-yellow-300">
        {menuItems.map(({ label, to, icon }) => {
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
                {icon}
                {label}
              </Link>
            </li>
          );
        })}
      </ul>
    </nav>
  );
}