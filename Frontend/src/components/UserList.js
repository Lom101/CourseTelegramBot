import React, { useState, useRef } from 'react';

const UserList = ({ users, onBlock, onDelete }) => {
  const [filter, setFilter] = useState({
    name: '',
    progress: '',
    status: '',
  });
  const [showRanges, setShowRanges] = useState(false);
  const progressInputRef = useRef();

  const handleRangeClick = (range) => {
    setFilter({ ...filter, progress: range });
    setShowRanges(false);
    progressInputRef.current.blur();
  };

  const getRangeMatch = (progressValue) => {
    const userProgress = parseInt(progressValue, 10);
    const filterValue = filter.progress.trim();

    if (filterValue.includes('-')) {
      const [min, max] = filterValue.split('-').map(str => parseInt(str, 10));
      return userProgress >= min && userProgress <= max;
    }

    if (!isNaN(parseInt(filterValue, 10))) {
      return userProgress === parseInt(filterValue, 10);
    }

    return true;
  };

  const filteredUsers = users.filter(user => {
    const nameMatch = user.name.toLowerCase().includes(filter.name.toLowerCase());
    const progressMatch = getRangeMatch(user.progress);
    const statusMatch =
      filter.status === '' ||
      (filter.status === 'Активен' && !user.blocked) ||
      (filter.status === 'Заблокирован' && user.blocked);
    return nameMatch && progressMatch && statusMatch;
  });

  return (
    <div className="max-w-5xl mt-6 px-4 ml-0 mr-auto">
      <div className="mb-4 flex flex-wrap gap-4 relative">
        <input
          type="text"
          placeholder="Поиск по ФИО"
          value={filter.name}
          onChange={(e) => setFilter({ ...filter, name: e.target.value })}
          className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
        />
        <div className="relative">
          <input
            type="text"
            placeholder="Прогресс (или выбрать)"
            value={filter.progress}
            ref={progressInputRef}
            onChange={(e) => setFilter({ ...filter, progress: e.target.value })}
            onFocus={() => setShowRanges(true)}
            onBlur={() => setTimeout(() => setShowRanges(false), 200)}
            className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
          />
          {showRanges && (
            <ul className="absolute left-0 w-full mt-1 bg-white border border-gray-300 rounded-xl shadow-md z-10">
              {['', '0-20', '21-40', '41-60', '61-80', '81-100'].map(range => (
                <li
                  key={range || 'none'}
                  onMouseDown={() => handleRangeClick(range)}
                  className="px-4 py-2 hover:bg-yellow-100 cursor-pointer text-sm"
                >
                  {range ? `${range}%` : 'Не выбрано'}
                </li>
              ))}
            </ul>
          )}
        </div>
        <select
          value={filter.status}
          onChange={(e) => setFilter({ ...filter, status: e.target.value })}
          className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm focus:outline-none focus:ring-2 focus:ring-yellow-400"
        >
          <option value="">Все статусы</option>
          <option value="Активен">Активен</option>
          <option value="Заблокирован">Заблокирован</option>
        </select>
      </div>

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
            {filteredUsers.map(user => (
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
                <td className="p-3">
                  <div className="flex flex-row items-center gap-2">
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
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default UserList;