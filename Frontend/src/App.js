import React from 'react';
import AdminDashboard from './pages/AdminDashboard';

<div className="mt-6 p-6 bg-white shadow rounded-xl">
  <h2 className="text-xl font-bold mb-4">Материалы</h2>
  <ul className="space-y-2">
    {[1, 2, 3, 4].map((blockId) => (
      <li
        key={blockId}
        className="text-blue-600 hover:underline cursor-pointer"
      >
        Материалы блока {blockId}
      </li>
    ))}
  </ul>
</div>

function App() {
  return (
    <div>
      <AdminDashboard />
    </div>
  );
}

export default App;