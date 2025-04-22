import React, { useState } from "react";
import usersData from "../data/users";
import UserList from "../components/UserList";

const UsersPage = () => {
  const [users, setUsers] = useState(usersData);

  const handleBlock = (id) => {
    setUsers(users.map(u => u.id === id ? { ...u, blocked: !u.blocked } : u));
  };

  const handleDelete = (id) => {
    setUsers(users.filter(u => u.id !== id));
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