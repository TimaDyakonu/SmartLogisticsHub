import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

import Sidebar from './components/Sidebar';
import AuthPage from './pages/AuthPage';
import DashboardPage from './pages/DashboardPage';
import InventoryPage from './pages/InventoryPage';
import OrdersPage from './pages/OrdersPage';
import TrackingPage from './pages/TrackingPage';
import { styles } from './styles';

export default function App() {
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));

    if (!token) {
        return (
            <Router>
                <Routes>
                    <Route path="*" element={<AuthPage setToken={setToken} />} />
                </Routes>
            </Router>
        );
    }

    return (
        <Router>
            <div style={styles.container}>
                <Sidebar setToken={setToken} />
                <div style={styles.content}>
                    <Routes>
                        <Route path="/" element={<DashboardPage />} />
                        <Route path="/inventory" element={<InventoryPage />} />
                        <Route path="/orders" element={<OrdersPage />} />
                        <Route path="/tracking" element={<TrackingPage />} />
                        <Route path="*" element={<Navigate to="/" />} />
                    </Routes>
                </div>
            </div>
        </Router>
    );
}