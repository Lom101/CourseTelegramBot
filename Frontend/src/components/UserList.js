import React from 'react';

const UserList = ({ users, onBlock, onDelete }) => (
  <div className="max-w-5xl mt-6 px-4 ml-0 mr-auto">
    <div className="overflow-x-auto rounded-2xl shadow-lg">
      <table className="min-w-full text-left bg-green-100 rounded-2xl overflow-hidden">
        <thead>
          <tr className="bg-green-200 text-gray-800">
            <th className="p-3 text-sm font-semibold">ФИО</th>
            <th className="p-3 text-sm font-semibold">Прогресс</th>
            <th className="p-3 text-sm font-semibold">Статус</th>
            <th className="p-3 text-sm font-semibold">Действия</th>
          </tr>
        </thead>
        <tbody>
          {users.map(user => (
            <tr key={user.id} className="border-t border-green-300 hover:bg-green-50 transition-colors">
              <td className="p-3 text-gray-800">{user.name}</td>
              <td className="p-3 text-gray-800">{user.progress}%</td>
              <td className="p-3 text-gray-800">
                {user.blocked ? (
                  <span className="text-red-600 font-medium">Заблокирован</span>
                ) : (
                  <span className="text-green-700 font-medium">Активен</span>
                )}
              </td>
              <td className="p-3 space-x-2">
                <button
                  onClick={() => onBlock(user.id)}
                  className="bg-orange-300 hover:bg-orange-400 text-gray-900 font-semibold px-3 py-1 rounded-full transition-colors"
                >
                  {user.blocked ? 'Разблокировать' : 'Заблокировать'}
                </button>
                <button
                  onClick={() => onDelete(user.id)}
                  className="bg-rose-400 hover:bg-rose-500 text-white font-semibold px-3 py-1 rounded-full transition-colors"
                >
                  Удалить
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  </div>
);

export default UserList;