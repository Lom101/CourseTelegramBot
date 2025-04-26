import React, { useEffect, useState } from "react";
import UserList from "../components/UserList";
import UserForm from "../components/UserForm";
import useApi from "../api/api";

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
  const [showForm, setShowForm] = useState(false); // Состояние для показа формы

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
      setShowForm(false); // Скрыть форму после успешного добавления
    } catch (error) {
      console.error("Ошибка при добавлении пользователя:", error);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 px-10 py-8">
      <div className="mb-8 text-center">
        <h2 className="text-4xl font-extrabold text-gray-800 mb-2">Список участников</h2>
        <div className="h-1 w-24 bg-gray-300 rounded-full mx-auto" />
      </div>

      {/* Кнопка показать/скрыть форму */}
      {isAdmin && (
        <div className="flex justify-center mb-6">
          <button
            onClick={() => setShowForm(!showForm)}
            className="bg-gray-400 hover:bg-gray-500 text-white font-semibold px-6 py-2 rounded-xl transition"
          >
            {showForm ? 'Скрыть форму' : 'Добавить пользователя'}
          </button>
        </div>
      )}

      {/* Форма добавления пользователя */}
      {showForm && (
        <div className="flex justify-center mb-10">
          <div className="w-full max-w-3xl">
            <UserForm
              newUser={newUser}
              setNewUser={setNewUser}
              handleAddUser={handleAddUser}
            />
          </div>
        </div>
      )}

      {/* Список пользователей на всю ширину */}
      <div className="w-full">
        <UserList
          users={users}
          onBlock={handleBlock}
          onDelete={handleDelete}
        />
      </div>
    </div>
  );
};

export default UsersPage;