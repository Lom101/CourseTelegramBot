import React from 'react';

const UserList = ({ users, onBlock, onDelete }) => (
  <div>
    <h2 className="text-2xl font-semibold mb-4 text-gray-800">Список участников</h2>
    <div className="overflow-x-auto">
      <table className="min-w-full text-left border border-gray-300 rounded-lg">
        <thead>
          <tr className="bg-gray-100">
            <th className="p-2">ФИО</th>
            <th className="p-2">Прогресс</th>
            <th className="p-2">Статус</th>
            <th className="p-2">Действия</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.id} className="border-t">
              <td className="p-2">{user.name}</td>
              <td className="p-2">{user.progress}%</td>
              <td className="p-2">{user.blocked ? 'Заблокирован' : 'Активен'}</td>
              <td className="p-2 space-x-2">
                <button
                  onClick={() => onBlock(user.id)}
                  className="bg-yellow-300 hover:bg-yellow-400 px-2 py-1 rounded"
                >
                  {user.blocked ? 'Разблокировать' : 'Заблокировать'}
                </button>
                <button
                  onClick={() => onDelete(user.id)}
                  className="bg-red-500 hover:bg-red-600 text-white px-2 py-1 rounded"
                >Удалить</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  </div>
);

export default UserList;