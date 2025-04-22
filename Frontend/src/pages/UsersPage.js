import React, { useState, useEffect } from "react";
import UserList from "../components/UserList";

const UsersPage = () => {
  const [users, setUsers] = useState([]);

  // Загрузка списка пользователей с API
  const fetchUsers = async () => {
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/api/Users`);
      const data = await response.json();
      setUsers(data);
    } catch (error) {
      console.error("Ошибка при загрузке пользователей:", error);
    }
  };

  // Блокировка пользователя
  const handleBlock = async (id) => {
    try {
      await fetch(`${process.env.REACT_APP_API_URL}/api/Users/${id}/block`, { method: "POST" });
      fetchUsers(); // обновляем список после действия
    } catch (error) {
      console.error("Ошибка при блокировке пользователя:", error);
    }
  };

  // Удаление пользователя
  const handleDelete = async (id) => {
    try {
      await fetch(`${process.env.REACT_APP_API_URL}/api/Users/${id}`, { method: "DELETE" });
      fetchUsers(); // обновляем список после удаления
    } catch (error) {
      console.error("Ошибка при удалении пользователя:", error);
    }
  };

  // Загружаем пользователей при монтировании компонента
  useEffect(() => {
    fetchUsers();
  }, []);

  return (
    <div className="min-h-screen bg-[#b9bedf] px-10 py-8">
      <div className="mb-6">
        <h2 className="text-4xl font-extrabold bg-gradient-to-r from-[#ffcc00] via-[#ffb703] to-[#ff9800] text-transparent bg-clip-text drop-shadow-md tracking-tight flex items-center gap-3">
          <span className="text-5xl">👥</span> Список участников
        </h2>
        <div className="h-1 w-24 bg-gradient-to-r from-[#ffcc00] to-[#ff9800] rounded-full mt-2" />
      </div>

      <div className="flex">
        <div className="w-full max-w-4xl">
          <UserList users={users} onBlock={handleBlock} onDelete={handleDelete} />
        </div>
      </div>
    </div>
  );
};

export default UsersPage;