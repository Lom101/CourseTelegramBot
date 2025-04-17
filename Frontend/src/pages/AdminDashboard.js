import React, { useState } from "react";
import usersData from "../data/users";
import UserList from "../components/UserList";
import ContentManager from "../components/ContentManager"; // ← добавили

const AdminDashboard = () => {
  const [users, setUsers] = useState(usersData);

  const handleBlock = (id) => {
    setUsers(users.map(u =>
      u.id === id ? { ...u, blocked: !u.blocked } : u
    ));
  };

  const handleDelete = (id) => {
    setUsers(users.filter(u => u.id !== id));
  };

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold mb-4">Админ-панель</h1>

      {/* Список участников */}
      <UserList users={users} onBlock={handleBlock} onDelete={handleDelete} />

      <hr className="my-6" />

      {/* Материалы по блокам */}
      <h2 className="text-2xl font-semibold mb-4">Материалы</h2>
      {["1", "2", "3", "4"].map((blockId) => (
        <div key={blockId} className="mb-8">
          <h3 className="text-xl font-semibold mb-2">Блок {blockId}</h3>
          <ContentManager blockId={`block${blockId}`} />
        </div>
      ))}
    </div>
  );
};

export default AdminDashboard;