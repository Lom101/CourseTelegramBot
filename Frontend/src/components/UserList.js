import React, { useState, useRef } from 'react';

const UserList = ({ users, onBlock, onDelete, onAdd }) => {
  const [filter, setFilter] = useState({ name: '', progress: '', status: '' });
  const [showRanges, setShowRanges] = useState(false);
  const progressInputRef = useRef();

  const [newUser, setNewUser] = useState({
    fullName: '',
    chatId: '',
    phoneNumber: '',
    email: '',
    progress: 0
  });

  const [isAdding, setIsAdding] = useState(false);

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
    const nameMatch = user.fullName.toLowerCase().includes(filter.name.toLowerCase());
    const progressMatch = getRangeMatch(user.progress);
    const statusMatch =
      filter.status === '' ||
      (filter.status === 'Активен' && !user.isBlocked) ||
      (filter.status === 'Заблокирован' && user.isBlocked);
    return nameMatch && progressMatch && statusMatch;
  });

  const handleAddUser = async () => {
    const { fullName, chatId, phoneNumber, email, progress } = newUser;
    if (!fullName.trim() || !chatId || !phoneNumber || !email) {
      alert('Заполните все поля');
      return;
    }

    setIsAdding(true);
    try {
      const response = await fetch(`${process.env.REACT_APP_API_URL}/api/Users/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          fullName,
          chatId: parseInt(chatId),
          phoneNumber,
          email,
          progress: parseInt(progress) || 0,
          isBlocked: false,
          isAdmin: false,
          registrationDate: new Date().toISOString(),
          lastActivity: new Date().toISOString()
        })
      });

      if (!response.ok) {
        const errorText = await response.text();
        console.error('Ошибка при добавлении пользователя:', errorText);
        throw new Error('Ошибка при добавлении пользователя');
      }

      setNewUser({ fullName: '', chatId: '', phoneNumber: '', email: '', progress: 0 });
      onAdd();
    } catch (error) {
      console.error(error);
      alert('Не удалось добавить пользователя');
    } finally {
      setIsAdding(false);
    }
  };

  return (
    <div className="max-w-6xl mt-6 px-4 ml-0 mr-auto">
      {/* Форма добавления пользователя */}
      <div className="mb-6 p-4 bg-yellow-50 border border-yellow-200 rounded-2xl">
        <h2 className="font-semibold mb-2 text-gray-800">Добавить пользователя</h2>
        <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
          <input type="text" placeholder="ФИО" value={newUser.fullName} onChange={(e) => setNewUser({ ...newUser, fullName: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          <input type="text" placeholder="Chat ID" value={newUser.chatId} onChange={(e) => setNewUser({ ...newUser, chatId: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          <input type="tel" placeholder="Телефон" value={newUser.phoneNumber} onChange={(e) => setNewUser({ ...newUser, phoneNumber: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          <input type="email" placeholder="Email" value={newUser.email} onChange={(e) => setNewUser({ ...newUser, email: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          <input type="number" placeholder="Прогресс (%)" value={newUser.progress} onChange={(e) => setNewUser({ ...newUser, progress: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          <button onClick={handleAddUser} disabled={isAdding} className="bg-green-500 hover:bg-green-600 text-white font-semibold px-4 py-2 rounded-xl transition col-span-2 md:col-span-1">
            {isAdding ? 'Добавление...' : 'Добавить'}
          </button>
        </div>
      </div>

      {/* Фильтры */}
      <div className="mb-4 flex flex-wrap gap-4 relative">
        <input type="text" placeholder="Поиск по ФИО" value={filter.name} onChange={(e) => setFilter({ ...filter, name: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
        <div className="relative">
          <input type="text" placeholder="Прогресс (или выбрать)" value={filter.progress} ref={progressInputRef} onChange={(e) => setFilter({ ...filter, progress: e.target.value })} onFocus={() => setShowRanges(true)} onBlur={() => setTimeout(() => setShowRanges(false), 200)} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm" />
          {showRanges && (
            <ul className="absolute left-0 w-full mt-1 bg-white border border-gray-300 rounded-xl shadow-md z-10">
              {['', '0-20', '21-40', '41-60', '61-80', '81-100'].map(range => (
                <li key={range || 'none'} onMouseDown={() => handleRangeClick(range)} className="px-4 py-2 hover:bg-yellow-100 cursor-pointer text-sm">
                  {range ? `${range}%` : 'Не выбрано'}
                </li>
              ))}
            </ul>
          )}
        </div>
        <select value={filter.status} onChange={(e) => setFilter({ ...filter, status: e.target.value })} className="px-4 py-2 rounded-xl border border-gray-300 shadow-sm">
          <option value="">Все статусы</option>
          <option value="Активен">Активен</option>
          <option value="Заблокирован">Заблокирован</option>
        </select>
      </div>

      {/* Таблица пользователей */}
      <div className="overflow-x-auto rounded-2xl shadow-lg">
        <table className="min-w-full text-left bg-green-100 rounded-2xl overflow-hidden">
          <thead>
            <tr className="bg-green-200 text-gray-800">
              <th className="p-3 text-sm font-semibold">ФИО</th>
              <th className="p-3 text-sm font-semibold">Email</th>
              <th className="p-3 text-sm font-semibold">Телефон</th>
              <th className="p-3 text-sm font-semibold">Прогресс</th>
              <th className="p-3 text-sm font-semibold">Статус</th>
              <th className="p-3 text-sm font-semibold">Последняя активность</th>
              <th className="p-3 text-sm font-semibold">Действия</th>
            </tr>
          </thead>
          <tbody>
            {filteredUsers.map(user => (
              <tr key={user.id} className="border-t border-green-300 hover:bg-green-50 transition-colors">
                <td className="p-3 text-gray-800">{user.fullName}</td>
                <td className="p-3 text-gray-800">{user.email}</td>
                <td className="p-3 text-gray-800">{user.phoneNumber}</td>
                <td className="p-3 text-gray-800">{user.progress}%</td>
                <td className="p-3 text-gray-800">{user.isBlocked ? 'Заблокирован' : 'Активен'}</td>
                <td className="p-3 text-gray-800">{new Date(user.lastActivity).toLocaleString()}</td>
                <td className="p-3">
                  <div className="flex flex-row items-center gap-2">
                    <button onClick={() => onBlock(user.id)} className="bg-orange-300 hover:bg-orange-400 text-gray-900 font-semibold px-3 py-1 rounded-full">
                      {user.isBlocked ? 'Разблокировать' : 'Заблокировать'}
                    </button>
                    <button onClick={() => onDelete(user.id)} className="bg-rose-400 hover:bg-rose-500 text-white font-semibold px-3 py-1 rounded-full">
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