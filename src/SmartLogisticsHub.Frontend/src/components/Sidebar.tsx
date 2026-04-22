import { Link } from 'react-router-dom';
import { styles } from '../styles';

interface SidebarProps { setToken: (token: string | null) => void; }

export default function Sidebar({ setToken }: SidebarProps) {
    return (
        <div style={styles.sidebar}>
            <div style={styles.logo}>📦 Smart Hub</div>
            <Link to="/" style={styles.link}>📊 Обзор (Dashboard)</Link>
            <Link to="/inventory" style={styles.link}>sck Склад (Inventory)</Link>
            <Link to="/orders" style={styles.link}>🛒 Заказы (Processing)</Link>
            <Link to="/tracking" style={styles.link}>📍 Отслеживание (Tracking)</Link>

            <div style={{ marginTop: 'auto' }}>
                <button style={{...styles.btnDanger, width: '100%'}} onClick={() => { localStorage.removeItem('token'); setToken(null); }}>
                    Выйти
                </button>
            </div>
        </div>
    );
}