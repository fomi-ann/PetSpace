import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import AppHeader from './AppHeader';
import AppSidebar from './AppSidebar';
import { navigationLinks } from '../../config/navigation';

const AppLayout = ({ children }) => {
    const [isSidebarOpen, setIsSidebarOpen] = useState(false);
    const navigate = useNavigate();

    const userRole = localStorage.getItem('userRole');
    const userName = localStorage.getItem('userFirstName') || '';

    const filteredLinks = useMemo(() => {
        return navigationLinks.filter(link => link.roles.includes(userRole));
    }, [userRole]);

    const handleLogout = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('userRole');
        localStorage.removeItem('userFirstName');
        navigate('/');
    };

    const openSidebar = () => setIsSidebarOpen(true);
    const closeSidebar = () => setIsSidebarOpen(false);

    return (
        <div className="min-vh-100 bg-light">
            <AppHeader
                onMenuClick={openSidebar}
                userName={userName}
                onLogout={handleLogout}
            />

            <AppSidebar
                isOpen={isSidebarOpen}
                onClose={closeSidebar}
                links={filteredLinks}
            />

            <main className="py-4">
                {children}
            </main>
        </div>
    );
};

export default AppLayout;