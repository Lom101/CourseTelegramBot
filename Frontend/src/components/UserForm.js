import React from "react";

const UserForm = ({ newUser, setNewUser, handleAddUser }) => {
  return (
    <div className="bg-white p-6 rounded-lg shadow-md mb-8 max-w-xl mx-auto">
      <h3 className="text-xl font-bold text-gray-800 mb-4">Добавить пользователя</h3>
      <div className="grid grid-cols-2 gap-4 mb-4">
        <input
          type="text"
          placeholder="ФИО"
          value={newUser.fullName}
          onChange={e => setNewUser({ ...newUser, fullName: e.target.value })}
          className="p-3 border border-gray-300 rounded"
        />
        <input
          type="text"
          placeholder="Телефон"
          value={newUser.phoneNumber}
          onChange={e => setNewUser({ ...newUser, phoneNumber: e.target.value })}
          className="p-3 border border-gray-300 rounded"
        />
        <input
          type="email"
          placeholder="Email"
          value={newUser.email}
          onChange={e => setNewUser({ ...newUser, email: e.target.value })}
          className="p-3 border border-gray-300 rounded"
        />
      </div>
      <button
        onClick={handleAddUser}
        className="w-full p-3 bg-gray-600 hover:bg-gray-700 text-white rounded"
      >
        Добавить
      </button>
    </div>
  );
};

export default UserForm;