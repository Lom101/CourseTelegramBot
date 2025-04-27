import React from 'react';

const EditModal = ({ editingUser, editData, handleEditChange, saveUserChanges, closeModal }) => {
  return (
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
        </div>

        <div className="flex justify-end mt-6 space-x-4">
          <button
            onClick={closeModal}
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
  );
};

export default EditModal;

