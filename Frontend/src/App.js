import React from "react";
import AdminDashboard  from "./pages/AdminDashboard";

function App() {
  return (
    <div className="container mx-auto">
      <h1 className="text-3xl font-bold text-center my-6">Панель администратора</h1>
      <AdminDashboard />
    </div>
  )
}

export default App;