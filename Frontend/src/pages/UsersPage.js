import React, { useEffect, useState } from "react";
import UserList from "../components/UserList";
import useApi from "../api";

const UsersPage = () => {
  const api = useApi();

  const [users, setUsers] = useState([]);
  const [isAdmin, setIsAdmin] = useState(false);
  const [newUser, setNewUser] = useState({
    phoneNumber: "",
    email: "",
    fullName: "",
    chatId: ""
  });

  useEffect(() => {
    checkAccess();
  }, []);

  const checkAccess = async () => {
    try {
      await api.get("/api/Auth/check");
      setIsAdmin(true);
      fetchUsers();
    } catch (err) {
      setIsAdmin(false);
    }
  };

  const fetchUsers = async () => {
    try {
      const response = await api.get("/api/User");
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
      const userToUpdate = users.find(u => u.id === id);
      const endpoint = userToUpdate.blocked
        ? `/api/User/${id}/unblock`
        : `/api/User/${id}/block`;
      await api.post(endpoint);
      setUsers(users.map(u => u.id === id ? { ...u, blocked: !u.blocked } : u));
    } catch (error) {
      console.error("Ошибка при (раз)блокировке пользователя:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      await api.delete(`/api/User/${id}`);
      setUsers(users.filter(u => u.id !== id));
    } catch (error) {
      console.error("Ошибка при удалении пользователя:", error);
    }
  };

  const handleAddUser = async () => {
    try {
      const payload = {
        phoneNumber: newUser.phoneNumber,
        email: newUser.email,
        fullName: newUser.fullName,
        chatId: parseInt(newUser.chatId)
      };
      await api.post("/api/User/add-new-user", payload);
      setNewUser({ phoneNumber: "", email: "", fullName: "", chatId: "" });
      fetchUsers();
    } catch (error) {
      console.error("Ошибка при добавлении пользователя:", error);
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

      {/* Форма добавления пользователя */}
      <div className="bg-white p-4 rounded-lg shadow-md mb-8 max-w-xl">
        <h3 className="text-xl font-bold mb-2">➕ Добавить пользователя</h3>
        <div className="grid grid-cols-2 gap-4 mb-4">
          <input type="text" placeholder="ФИО" value={newUser.fullName} onChange={e => setNewUser({ ...newUser, fullName: e.target.value })} className="p-2 border rounded" />
          <input type="text" placeholder="Телефон" value={newUser.phoneNumber} onChange={e => setNewUser({ ...newUser, phoneNumber: e.target.value })} className="p-2 border rounded" />
          <input type="email" placeholder="Email" value={newUser.email} onChange={e => setNewUser({ ...newUser, email: e.target.value })} className="p-2 border rounded" />
          <input type="text" placeholder="Chat ID" value={newUser.chatId} onChange={e => setNewUser({ ...newUser, chatId: e.target.value })} className="p-2 border rounded" />
        </div>
        <button onClick={handleAddUser} className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded">
          Добавить
        </button>
      </div>

      {/* Список пользователей */}
      <div className="flex">
        <div className="w-full max-w-4xl">
          <UserList
            users={users}
            onBlock={handleBlock}
            onDelete={handleDelete}
          />
        </div>
      </div>
    </div>
  );
};

export default UsersPage;