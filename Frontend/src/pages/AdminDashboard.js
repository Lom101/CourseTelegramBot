//главная страница админки

import React, {useState} from "react";
import usersData from "../data/users";
import UserList from "../components/UserList";
import ContentManager from '../components/ContentManager';

const AdminDashboard = () => {
    const [users, setUsers] = useState(usersData);

    const handleBlock = (id) => {
        setUsers(users.map(u =>
            u.id === id ? {...u, blocked: !u.blocked} : u
        ));
    };

    const handleDelete = (id) => {
        setUsers(users.filter(u => u.id !== id));
    };
    
    return (
        <div className="p-6 space-y-8">
            <UserList users={users} onBlock={handleBlock} onDelete={handleDelete} />
            <ContentManager />
        </div>
    );
};

export default AdminDashboard;