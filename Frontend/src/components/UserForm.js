import React, { useState } from "react";

const UserForm = ({ newUser, setNewUser, handleAddUser }) => {
  const [errors, setErrors] = useState({});

  const validate = () => {
    const newErrors = {};
    if (!newUser.phoneNumber.startsWith("+")) {
      newErrors.phoneNumber = "Телефон должен начинаться с '+'";
    }
    if (!newUser.email.includes("@")) {
      newErrors.email = "Некорректный email";
    }
    if (!newUser.fullName.trim()) {
      newErrors.fullName = "ФИО не должно быть пустым";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = () => {
    if (validate()) {
      handleAddUser();
    }
  };

  return (
    <div className="flex flex-wrap items-center justify-center gap-4 mb-8">
      <div className="flex flex-col">
        <input
          type="text"
          placeholder="ФИО"
          value={newUser.fullName}
          onChange={e => setNewUser({ ...newUser, fullName: e.target.value })}
          className={`px-4 py-2 border ${
            errors.fullName ? "border-rose-500" : "border-gray-300"
          } rounded-xl focus:outline-none focus:ring-2 focus:ring-gray-300`}
        />
        {errors.fullName && <span className="text-rose-500 text-sm mt-1">{errors.fullName}</span>}
      </div>
      
      <div className="flex flex-col">
        <input
          type="text"
          placeholder="Телефон"
          value={newUser.phoneNumber}
          onChange={e => setNewUser({ ...newUser, phoneNumber: e.target.value })}
          className={`px-4 py-2 border ${
            errors.phoneNumber ? "border-rose-500" : "border-gray-300"
          } rounded-xl focus:outline-none focus:ring-2 focus:ring-gray-300`}
        />
        {errors.phoneNumber && <span className="text-rose-500 text-sm mt-1">{errors.phoneNumber}</span>}
      </div>

      <div className="flex flex-col">
        <input
          type="email"
          placeholder="Email"
          value={newUser.email}
          onChange={e => setNewUser({ ...newUser, email: e.target.value })}
          className={`px-4 py-2 border ${
            errors.email ? "border-rose-500" : "border-gray-300"
          } rounded-xl focus:outline-none focus:ring-2 focus:ring-gray-300`}
        />
        {errors.email && <span className="text-rose-500 text-sm mt-1">{errors.email}</span>}
      </div>

      <button
        onClick={handleSubmit}
        className="px-6 py-2 bg-gray-700 hover:bg-gray-800 text-white font-semibold rounded-xl transition h-fit"
      >
        Добавить
      </button>
    </div>
  );
};

export default UserForm;