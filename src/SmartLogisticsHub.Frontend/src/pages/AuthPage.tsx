import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { API_URL } from '../config';
import { styles } from '../styles';

interface AuthPageProps {
    setToken: (token: string) => void;
}

export default function AuthPage({ setToken }: AuthPageProps) {
    const[email, setEmail] = useState<string>('test@test.com');
    const[password, setPassword] = useState<string>('123456');
    const navigate = useNavigate();

    const handleAuth = async (isLogin: boolean) => {
        const endpoint = isLogin ? '/auth/login' : '/auth/register';
        const res = await fetch(`${API_URL}${endpoint}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ email, password })
        });
        const data = await res.json();

        if (data.token) {
            localStorage.setItem('token', data.token);
            setToken(data.token);
            navigate('/');
        } else {
            alert('Ошибка! ' + JSON.stringify(data));
        }
    };

    return (
        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh', backgroundColor: '#f8fafc' }}>
            <div style={{ ...styles.card, width: '400px' }}>
                <h2>Smart Logistics Hub</h2>
                <p>Необходима авторизация</p>
                <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <input style={styles.input} type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" />
                    <input style={styles.input} type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" />
                    <button style={styles.btn} onClick={() => handleAuth(true)}>Login</button>
                    <button style={{...styles.btn, backgroundColor: '#10b981'}} onClick={() => handleAuth(false)}>Register</button>
                </div>
            </div>
        </div>
    );
}