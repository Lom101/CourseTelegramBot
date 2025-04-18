import React, { useState } from "react";
import usersData from "../data/users";
import UserList from "../components/UserList";
import ContentManager from "../components/ContentManager";

const AdminDashboard = () => {
  const [users, setUsers] = useState(usersData);
  const [openBlocks, setOpenBlocks] = useState([]);
  const blocks = ["1", "2", "3", "4"];

  const handleBlock = (id) => {
    setUsers(users.map(u => u.id === id ? { ...u, blocked: !u.blocked } : u));
  };

  const handleDelete = (id) => {
    setUsers(users.filter(u => u.id !== id));
  };

  const toggleBlock = (blockId) => {
    setOpenBlocks((prev) =>
      prev.includes(blockId)
        ? prev.filter(id => id !== blockId)
        : [...prev, blockId]
    );
  };

  return (
    <div className="p-4">
      <h1 className="text-3xl font-bold mb-6">Админ-панель</h1>

      {/* Список участников */}
      <UserList users={users} onBlock={handleBlock} onDelete={handleDelete} />

      <hr className="my-6" />

      <h2 className="text-2xl font-semibold mb-4">Материалы</h2>
      <div className="flex flex-wrap gap-4">
        {blocks.map((blockId) => (
          <div
            key={blockId}
            className="border p-4 rounded-xl shadow w-[300px] min-w-[250px] bg-white"
          >
            <h3
              onClick={() => toggleBlock(blockId)}
              className="text-lg font-semibold mb-2 cursor-pointer hover:underline"
            >
              Блок {blockId}
            </h3>

            {openBlocks.includes(blockId) && (
              <div className="mt-2 border border-gray-300 rounded-lg p-3 bg-white/70 backdrop-blur-sm">
                <ContentManager blockId={blockId} />
              </div>
            )}
          </div>
        ))}
      </div>
    </div>
  );
};

export default AdminDashboard;