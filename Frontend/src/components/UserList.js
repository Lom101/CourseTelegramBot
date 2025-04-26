import React, { useState, useEffect } from 'react';

const API_URL = 'http://localhost:5000/api/User';

const UserList = () => {
  const [users, setUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);
  const [editData, setEditData] = useState({
    id: '',
    phoneNumber: '',
    email: '',
    fullName: '',
    chatId: '',
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    const response = await fetch(API_URL);
    const data = await response.json();
    setUsers(data);
  };

  const blockUser = async (id) => {
    await fetch(`${API_URL}/${id}/block`, { method: 'POST' });
    fetchUsers();
  };

  const unblockUser = async (id) => {
    await fetch(`${API_URL}/${id}/unblock`, { method: 'POST' });
    fetchUsers();
  };

  const deleteUser = async (id) => {
    if (window.confirm('Вы уверены, что хотите удалить пользователя?')) {
      await fetch(`${API_URL}/${id}`, { method: 'DELETE' });
      fetchUsers();
    }
  };

  const openEditModal = (user) => {
    setEditingUser(user);
    setEditData({
      id: user.id,
      phoneNumber: user.phoneNumber,
      email: user.email,
      fullName: user.fullName,
      chatId: user.chatId ?? '',
    });
  };

  const handleEditChange = (e) => {
    const { name, value } = e.target;
    setEditData((prev) => ({ ...prev, [name]: value }));
  };

  const saveUserChanges = async () => {
    const updatedUser = {
      id: editingUser.id,
      phoneNumber: editData.phoneNumber,
      email: editData.email,
      fullName: editData.fullName,
      chatId: editData.chatId || null,
    };

    await fetch(`${API_URL}/${editingUser.id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updatedUser),
    });

    setEditingUser(null);
    fetchUsers();
  };

  return (
    <div className="max-w-7xl mx-auto mt-6 px-4">
      <h1 className="text-2xl font-bold mb-4 text-gray-700">Пользователи</h1>

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
              <tr key={user.id} className="border-t border-gray-300 hover:bg-gray-50 transition-colors">
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
))}
</tbody>
</table>
</div>

{/* Модалка для редактирования */}
{editingUser && (
<div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
<div className="bg-white rounded-2xl p-6 w-full max-w-md">
<h2 className="text-xl font-bold mb-4 text-gray-700">Редактировать пользователя</h2>

<div className="space-y-4">
<input
type="text"
name="fullName"
placeholder="ФИО"
value={editData.fullName}
onChange={handleEditChange}
className="w-full px-4 py-2 border rounded-xl focus:outline-none bg-gray-100"
/>
<input
type="text"
name="phoneNumber"
placeholder="Телефон"
value={editData.phoneNumber}
onChange={handleEditChange}
className="w-full px-4 py-2 border rounded-xl focus:outline-none bg-gray-100"
/>
<input
type="email"
name="email"
placeholder="Email"
value={editData.email}
onChange={handleEditChange}
className="w-full px-4 py-2 border rounded-xl focus:outline-none bg-gray-100"
/>
<input
type="text"
name="chatId"
placeholder="ChatId"
value={editData.chatId}
onChange={handleEditChange}
className="w-full px-4 py-2 border rounded-xl focus:outline-none bg-gray-100"
/>
</div>

<div className="flex justify-end mt-6 space-x-4">
<button
onClick={() => setEditingUser(null)}
className="px-4 py-2 rounded-xl bg-gray-300 hover:bg-gray-400 text-gray-700 font-semibold"
>
Отмена
</button>
<button
onClick={saveUserChanges}
className="px-4 py-2 rounded-xl bg-gray-500 hover:bg-gray-600 text-white font-semibold"
>
Сохранить
</button>
</div>
</div>
</div>
)}
</div>
);
};

export default UserList;