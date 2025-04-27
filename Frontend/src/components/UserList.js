import React, { useState, useEffect } from 'react';
import UserTable from './UserTable';
import EditModal from './EditModal';


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
      <UserTable
        users={users}
        blockUser={blockUser}
        unblockUser={unblockUser}
        deleteUser={deleteUser}
        openEditModal={openEditModal}
      />
      
      {/* Модалка для редактирования */}
      {editingUser && (
        <EditModal
          editingUser={editingUser}
          editData={editData}
          handleEditChange={handleEditChange}
          saveUserChanges={saveUserChanges}
          closeModal={() => setEditingUser(null)}
        />
      )}
    </div>
  );
  
};

export default UserList; 
