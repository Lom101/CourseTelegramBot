import React, { useEffect, useState } from "react";
import axios from "axios";
import UserList from "../components/UserList";

const UsersPage = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axios.get("http://localhost:5000/api/User");
      const data = response.data.map(user => ({
        id: user.id,
        name: user.fullName,
        progress: user.progress ?? 0,
        blocked: user.isBlocked ?? false
      }));
      setUsers(data);
    } catch (error) {
      console.error("Ошибка при загрузке пользователей:", error);
    }
  };

  const handleBlock = async (id) => {
    try {
      const user = users.find(u => u.id === id);
      const endpoint = user.blocked
        ? `http://localhost:5000/api/User/${id}/unblock`
        : `http://localhost:5000/api/User/${id}/block`;
      await axios.post(endpoint);
      setUsers(users.map(u => u.id === id ? { ...u, blocked: !u.blocked } : u));
    } catch (error) {
      console.error("Ошибка при блокировке/разблокировке пользователя:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await axios.delete(`http://localhost:5000/api/User/${id}`);
      setUsers(users.filter(u => u.id !== id));
    } catch (error) {
      console.error("Ошибка при удалении пользователя:", error);
    }
  };

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
