import React from 'react';

const UserRow = ({ user, blockUser, unblockUser, deleteUser, openEditModal }) => {
  return (
    <tr className="border-t border-gray-300 hover:bg-gray-50 transition-colors">
      <td className="p-3">{user.fullName}</td>
      <td className="p-3">{user.phoneNumber}</td>
      <td className="p-3">{user.email}</td>
      <td className="p-3">{user.chatId ?? '—'}</td>
      <td className="p-3">{new Date(user.registrationDate).toLocaleString()}</td>
      <td className="p-3">{user.lastActivity ? new Date(user.lastActivity).toLocaleString() : '—'}</td>
      <td className="p-3">{user.isAdmin ? 'Администратор' : 'Пользователь'}</td>
      <td className="p-3">
        {user.isBlocked ? (
          <span className="text-red-600 font-semibold">Заблокирован</span>
        ) : (
          <span className="text-green-700 font-semibold">Активен</span>
        )}
      </td>
      <td className="p-3 space-y-2 flex flex-col">
        <button
          onClick={() => openEditModal(user)}
          className="bg-gray-400 hover:bg-gray-500 text-white font-semibold px-3 py-1 rounded-full transition"
        >
          Редактировать
        </button>
        <button
          onClick={() => user.isBlocked ? unblockUser(user.id) : blockUser(user.id)}
          className="bg-gray-400 hover:bg-gray-500 text-white font-semibold px-3 py-1 rounded-full transition"
        >
          {user.isBlocked ? 'Разблокировать' : 'Заблокировать'}
        </button>
        <button
          onClick={() => deleteUser(user.id)}
          className="bg-gray-400 hover:bg-gray-500 text-white font-semibold px-3 py-1 rounded-full transition"
        >
          Удалить
        </button>
      </td>
    </tr>
  );
};

export default UserRow;
