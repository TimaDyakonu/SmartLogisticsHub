import { useState, useEffect } from 'react';
import { API_URL, type Item } from '../config';
import { styles } from '../styles';

export default function InventoryPage() {
    const [items, setItems] = useState<Item[]>([]);
    const [isFiltered, setIsFiltered] = useState(false);

    const [newItem, setNewItem] = useState({ name: '', sku: '', weight: 0, volume: 0, category: '' });

    const fetchItems = async (heavyOnly: boolean) => {
        const endpoint = heavyOnly ? '/behavioral/iterator' : '/inventory';
        const res = await fetch(`${API_URL}${endpoint}`);
        setItems(await res.json());
        setIsFiltered(heavyOnly);
    };

    useEffect(() => { fetchItems(false); },[]);

    const handleCreate = async () => {
        if (!newItem.name || !newItem.sku) return alert('Заполните Название и SKU');
        await fetch(`${API_URL}/inventory`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newItem)
        });
        setNewItem({ name: '', sku: '', weight: 0, volume: 0, category: '' });
        fetchItems(isFiltered);
    };

    const handleClone = async (id: string, currentSku: string) => {
        const newSku = prompt('Введите новый SKU для копии:', currentSku + '-COPY');
        if (!newSku) return;

        await fetch(`${API_URL}/inventory/clone/${id}?newSku=${newSku}`, { method: 'POST' });
        fetchItems(isFiltered);
    };

    return (
        <div>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '25px' }}>
                <h1 style={{ ...styles.header, margin: 0 }}>Управление складом</h1>
                <button
                    style={isFiltered ? styles.btnDanger : styles.btn}
                    onClick={() => fetchItems(!isFiltered)}
                >
                    {isFiltered ? 'Показать все товары' : 'Фильтр: Только тяжелые (>50кг)'}
                    {!isFiltered && <span style={{marginLeft: '10px', fontSize: '12px'}}>*Iterator Pattern</span>}
                </button>
            </div>

            <div style={{ ...styles.card, backgroundColor: '#f8fafc', border: '1px solid #e2e8f0' }}>
                <h3 style={{ marginTop: 0, marginBottom: '15px' }}>Приемка нового товара</h3>
                <div style={{ display: 'flex', gap: '10px', flexWrap: 'wrap' }}>
                    <input style={{...styles.input, flex: 1}} placeholder="SKU (напр. MT-01)" value={newItem.sku} onChange={e => setNewItem({...newItem, sku: e.target.value})} />
                    <input style={{...styles.input, flex: 2}} placeholder="Название (напр. Steel Beam)" value={newItem.name} onChange={e => setNewItem({...newItem, name: e.target.value})} />
                    <input style={{...styles.input, flex: 1}} placeholder="Категория" value={newItem.category} onChange={e => setNewItem({...newItem, category: e.target.value})} />
                    <input style={{...styles.input, flex: 1}} type="number" placeholder="Вес (кг)" value={newItem.weight || ''} onChange={e => setNewItem({...newItem, weight: Number(e.target.value)})} />
                    <button style={styles.btn} onClick={handleCreate}>Добавить на склад</button>
                </div>
            </div>

            <div style={styles.card}>
                <table style={styles.table}>
                    <thead>
                    <tr>
                        <th style={styles.th}>SKU</th>
                        <th style={styles.th}>Название</th>
                        <th style={styles.th}>Категория</th>
                        <th style={styles.th}>Вес (кг)</th>
                        <th style={styles.th}>Действия</th>
                    </tr>
                    </thead>
                    <tbody>
                    {items.map(item => (
                        <tr key={item.id}>
                            <td style={styles.td}><b>{item.sku}</b></td>
                            <td style={styles.td}>{item.name}</td>
                            <td style={styles.td}><span style={styles.badge}>{item.category || 'N/A'}</span></td>
                            <td style={styles.td}>{item.weight}</td>
                            <td style={styles.td}>
                                <button style={styles.btnOutline} onClick={() => handleClone(item.id, item.sku)}>
                                    Копировать (Prototype)
                                </button>
                            </td>
                        </tr>
                    ))}
                    {items.length === 0 && <tr><td colSpan={5} style={{...styles.td, textAlign: 'center', color: '#94a3b8'}}>Склад пуст. Выполните приемку товара выше.</td></tr>}
                    </tbody>
                </table>
            </div>
        </div>
    );
}