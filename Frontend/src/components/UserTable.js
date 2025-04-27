import React from 'react';
import UserRow from './UserRow';

const UserTable = ({ users, blockUser, unblockUser, deleteUser, openEditModal }) => {
  return (
    <div className="overflow-x-auto rounded-2xl shadow-lg">
      <table className="min-w-full text-left bg-gray-100 rounded-2xl overflow-hidden">
        <thead>
          <tr className="bg-gray-300 text-gray-800">
            <th className="p-3 text-sm font-semibold">ФИО</th>
            <th className="p-3 text-sm font-semibold">Телефон</th>
            <th className="p-3 text-sm font-semibold">Email</th>
            <th className="p-3 text-sm font-semibold">ChatId</th>
            <th className="p-3 text-sm font-semibold">Дата регистрации</th>
            <th className="p-3 text-sm font-semibold">Последняя активность</th>
            <th className="p-3 text-sm font-semibold">Роль</th>
            <th className="p-3 text-sm font-semibold">Статус</th>
            <th className="p-3 text-sm font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <UserRow
              key={user.id}
              user={user}
              blockUser={blockUser}
              unblockUser={unblockUser}
              deleteUser={deleteUser}
              openEditModal={openEditModal}
            />
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default UserTable;
