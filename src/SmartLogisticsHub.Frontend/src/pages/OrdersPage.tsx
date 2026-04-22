import { useState, useEffect } from 'react';
import { API_URL, type Order } from '../config';
import { styles } from '../styles';

export default function OrdersPage() {
    const [orders, setOrders] = useState<Order[]>([]);
    const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);

    const [newCustomerName, setNewCustomerName] = useState('');

    const [shippingWeight, setShippingWeight] = useState<number>(10);
    const[shippingCost, setShippingCost] = useState<number | null>(null);
    const [logs, setLogs] = useState<string[]>([]);

    const fetchOrders = async () => {
        const res = await fetch(`${API_URL}/logistics/orders`);
        setOrders(await res.json());
    };

    useEffect(() => { fetchOrders(); },[]);

    const handleCreateOrder = async () => {
        if (!newCustomerName) return alert('Введите имя клиента');
        await fetch(`${API_URL}/logistics/orders`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ customerName: newCustomerName, status: 'New' })
        });
        setNewCustomerName('');
        fetchOrders();
    };

    const saveOrderToDb = async (updatedOrder: Order) => {
        await fetch(`${API_URL}/logistics/orders`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updatedOrder)
        });
        fetchOrders();
        setSelectedOrder(updatedOrder);
    };

    const processState = async () => {
        if (!selectedOrder) return;
        const res = await fetch(`${API_URL}/behavioral/state`, {
            method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(selectedOrder)
        });
        const updated = await res.json();
        saveOrderToDb(updated);
    };

    const applyTemplate = async (isVip: boolean) => {
        if (!selectedOrder) return;
        const res = await fetch(`${API_URL}/behavioral/template?isVip=${isVip}`, {
            method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(selectedOrder)
        });
        const updated = await res.json();
        saveOrderToDb(updated);
    };

    const dispatchAndNotify = async () => {
        if (!selectedOrder) return;
        const res = await fetch(`${API_URL}/behavioral/observer`, {
            method: 'POST', headers: {'Content-Type': 'application/json'}, body: JSON.stringify(selectedOrder)
        });
        setLogs(await res.json());
    };

    const calculateShipping = async (type: string) => {
        const res = await fetch(`${API_URL}/behavioral/strategy?weight=${shippingWeight}&type=${type}`);
        const data = await res.json();
        setShippingCost(data.cost);
    };

    return (
        <div>
            <h1 style={styles.header}>Управление заказами</h1>

            <div style={{ display: 'flex', gap: '20px', alignItems: 'flex-start' }}>

                <div style={{ ...styles.card, flex: 1.5 }}>
                    <h3 style={{ marginTop: 0 }}>Регистрация нового заказа</h3>
                    <div style={{ display: 'flex', gap: '10px', marginBottom: '20px' }}>
                        <input
                            style={{...styles.input, marginBottom: 0}}
                            placeholder="Имя клиента"
                            value={newCustomerName}
                            onChange={e => setNewCustomerName(e.target.value)}
                        />
                        <button style={{...styles.btn, backgroundColor: '#10b981'}} onClick={handleCreateOrder}>
                            Создать
                        </button>
                    </div>

                    <h3 style={{ marginTop: 0, borderTop: '1px solid #e2e8f0', paddingTop: '15px' }}>Входящий поток (Из БД)</h3>
                    <table style={styles.table}>
                        <thead>
                        <tr>
                            <th style={styles.th}>ID Заказа</th>
                            <th style={styles.th}>Клиент</th>
                            <th style={styles.th}>Статус</th>
                            <th style={styles.th}>Действия</th>
                        </tr>
                        </thead>
                        <tbody>
                        {orders.map(o => (
                            <tr key={o.id} style={{ backgroundColor: selectedOrder?.id === o.id ? '#f0fdf4' : 'transparent' }}>
                                <td style={styles.td}><span title={o.id}>{o.id.substring(0, 8)}...</span></td>
                                <td style={styles.td}><b>{o.customerName}</b></td>
                                <td style={styles.td}>
                    <span style={{
                        ...styles.badge,
                        backgroundColor: o.status === 'New' ? '#dbeafe' : o.status === 'Processing' ? '#fef08a' : '#d1fae5',
                        color: o.status === 'New' ? '#1e40af' : o.status === 'Processing' ? '#854d0e' : '#065f46'
                    }}>
                      {o.status}
                    </span>
                                </td>
                                <td style={styles.td}>
                                    <button
                                        style={selectedOrder?.id === o.id ? styles.btn : styles.btnOutline}
                                        onClick={() => { setSelectedOrder(o); setLogs([]); }}
                                    >
                                        {selectedOrder?.id === o.id ? 'Выбрано' : 'Управление'}
                                    </button>
                                </td>
                            </tr>
                        ))}
                        {orders.length === 0 && (
                            <tr>
                                <td colSpan={4} style={{...styles.td, textAlign: 'center', color: '#94a3b8'}}>
                                    База пуста. Зарегистрируйте первый заказ выше.
                                </td>
                            </tr>
                        )}
                        </tbody>
                    </table>
                </div>

                <div style={{ display: 'flex', flexDirection: 'column', gap: '20px', flex: 1 }}>

                    <div style={{ ...styles.card, margin: 0, minHeight: '350px' }}>
                        <h3 style={{ marginTop: 0 }}>Панель оператора</h3>

                        {!selectedOrder ? (
                            <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '200px', color: '#94a3b8' }}>
                                👈 Выберите заказ из БД слева
                            </div>
                        ) : (
                            <div>
                                <div style={{ padding: '15px', border: '1px solid #e2e8f0', borderRadius: '8px', backgroundColor: '#f8fafc' }}>
                                    <p><b>Заказ:</b> {selectedOrder.id}</p>
                                    <p><b>Клиент:</b> {selectedOrder.customerName}</p>
                                    <p><b>Текущий статус:</b> {selectedOrder.status}</p>
                                </div>

                                <h4 style={{ marginTop: '20px', marginBottom: '10px', fontSize: '14px', color: '#64748b' }}>АЛГОРИТМЫ ОБРАБОТКИ:</h4>
                                <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                                    <button style={styles.btnOutline} onClick={processState}>
                                        Сдвинуть по конвейеру (State Pattern)
                                    </button>

                                    <div style={{ display: 'flex', gap: '10px' }}>
                                        <button style={{...styles.btnOutline, flex: 1}} onClick={() => applyTemplate(false)}>
                                            Standard (Template)
                                        </button>
                                        <button style={{...styles.btnOutline, flex: 1, borderColor: '#8b5cf6', color: '#8b5cf6'}} onClick={() => applyTemplate(true)}>
                                            VIP (Template)
                                        </button>
                                    </div>

                                    <button style={{...styles.btn, marginTop: '10px', backgroundColor: '#10b981'}} onClick={dispatchAndNotify}>
                                        Отправить уведомления (Observer Pattern)
                                    </button>
                                </div>

                                {logs.length > 0 && (
                                    <div style={styles.logBox}>
                                        {logs.map((log, i) => <div key={i}>{log}</div>)}
                                    </div>
                                )}
                            </div>
                        )}
                    </div>

                    <div style={{ ...styles.card, margin: 0 }}>
                        <h3 style={{ marginTop: 0 }}>Логистика <span style={{fontSize:'12px', fontWeight:'normal', color: '#64748b'}}>(Strategy)</span></h3>
                        <div style={styles.formGroup}>
                            <label style={{ display: 'block', marginBottom: '8px', fontSize: '13px' }}>Вес груза (кг):</label>
                            <input style={styles.input} type="number" value={shippingWeight} onChange={e => setShippingWeight(Number(e.target.value))} />
                        </div>
                        <div style={{ display: 'flex', gap: '10px' }}>
                            <button style={{...styles.btnOutline, flex: 1}} onClick={() => calculateShipping('standard')}>Standard</button>
                            <button style={{...styles.btnOutline, flex: 1, borderColor: '#f59e0b', color: '#f59e0b'}} onClick={() => calculateShipping('express')}>Express</button>
                        </div>

                        {shippingCost !== null && (
                            <div style={{ marginTop: '15px', padding: '10px', backgroundColor: '#f0fdf4', color: '#166534', borderRadius: '6px', textAlign: 'center', fontWeight: 'bold' }}>
                                Итого: ${shippingCost.toFixed(2)}
                            </div>
                        )}
                    </div>

                </div>
            </div>
        </div>
    );
}