import { useState } from 'react';
import { API_URL } from '../config';
import { styles } from '../styles';

export default function TrackingPage() {
    const [trackingId, setTrackingId] = useState('LEGACY-00123');
    const [legacyStatus, setLegacyStatus] = useState<any>(null);

    const [cargoId, setCargoId] = useState('');
    const [cargoData, setCargoData] = useState<any>(null);

    const [newCargo, setNewCargo] = useState({ name: '', weight: 0, isBundle: false, parentId: '' });
    const[createdRootId, setCreatedRootId] = useState<string | null>(null);

    const checkLegacy = async () => {
        const res = await fetch(`${API_URL}/logistics/shipment/status/${trackingId}`);
        setLegacyStatus(await res.json());
    };

    const checkComposite = async () => {
        if(!cargoId) return alert('Введите GUID Cargo Bundle');
        const res = await fetch(`${API_URL}/logistics/cargo/${cargoId}`);
        if(res.ok) setCargoData(await res.json());
        else alert('Bundle не найден!');
    };

    const createCargo = async () => {
        const payload = { ...newCargo, parentId: newCargo.parentId || null };
        const res = await fetch(`${API_URL}/logistics/cargo`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (!res.ok) return alert(`Ошибка создания: ${res.status}`);

        const data = await res.json();
        const newId = data.id || data.Id;

        if (newCargo.isBundle) {
            setCreatedRootId(newId);
            setCargoId(newId);
        }

        alert(`Успешно создано! ID: ${newId}`);
        setNewCargo({ name: '', weight: 0, isBundle: false, parentId: '' });
    };

    return (
        <div>
            <h1 style={styles.header}>Отслеживание и инспекция грузов</h1>

            <div style={{ ...styles.card, backgroundColor: '#f8fafc', border: '1px dashed #94a3b8' }}>
                <h3 style={{ marginTop: 0 }}>Конструктор отгрузки (Формирование Composite)</h3>
                <p style={{ fontSize: '13px', color: '#64748b' }}>Создайте контейнер (Бандл), а затем добавьте в него обычные товары, указав Parent ID контейнера.</p>

                <div style={{ display: 'flex', gap: '10px', alignItems: 'center', flexWrap: 'wrap', marginTop: '15px' }}>
                    <input style={{...styles.input, flex: 2, marginBottom: 0}} placeholder="Название (напр. Паллета #1)" value={newCargo.name} onChange={e => setNewCargo({...newCargo, name: e.target.value})} />
                    <input style={{...styles.input, flex: 1, marginBottom: 0}} type="number" placeholder="Вес (кг)" value={newCargo.weight || ''} onChange={e => setNewCargo({...newCargo, weight: Number(e.target.value)})} disabled={newCargo.isBundle} />

                    <label style={{ display: 'flex', alignItems: 'center', gap: '5px', fontSize: '14px', cursor: 'pointer' }}>
                        <input type="checkbox" checked={newCargo.isBundle} onChange={e => setNewCargo({...newCargo, isBundle: e.target.checked, weight: 0})} />
                        Это контейнер (Bundle)
                    </label>

                    <input style={{...styles.input, flex: 2, marginBottom: 0}} placeholder="Parent ID (если кладем внутрь)" value={newCargo.parentId} onChange={e => setNewCargo({...newCargo, parentId: e.target.value})} />

                    <button style={{...styles.btn, backgroundColor: '#10b981'}} onClick={createCargo}>Сформировать</button>
                </div>

                {createdRootId && (
                    <div style={{ marginTop: '10px', fontSize: '14px', color: '#059669', padding: '10px', backgroundColor: '#d1fae5', borderRadius: '4px' }}>
                        <b>Последний созданный контейнер:</b> {createdRootId}
                    </div>
                )}
            </div>

            <div style={{ display: 'flex', gap: '20px' }}>
                <div style={{ ...styles.card, flex: 1 }}>
                    <h3>Старая система трекинга <span style={styles.badge}>Adapter</span></h3>
                    <p style={{ fontSize: '13px', color: '#64748b', marginBottom: '15px' }}>Подключение к внешнему API (Legacy Shipping)</p>
                    <input style={styles.input} value={trackingId} onChange={e => setTrackingId(e.target.value)} placeholder="Tracking ID" />
                    <button style={{...styles.btn, marginTop: '10px'}} onClick={checkLegacy}>Проверить статус</button>

                    {legacyStatus && (
                        <div style={{ marginTop: '15px', padding: '15px', border: '1px solid #e2e8f0', borderRadius: '8px' }}>
                            <p><b>Провайдер:</b> Legacy Shipping Co.</p>
                            <p><b>Ответ:</b> <span style={{color: '#2563eb'}}>{legacyStatus.status}</span></p>
                        </div>
                    )}
                </div>

                <div style={{ ...styles.card, flex: 1 }}>
                    <h3>Инспекция контейнеров <span style={styles.badge}>Composite</span></h3>
                    <p style={{ fontSize: '13px', color: '#64748b', marginBottom: '15px' }}>Рекурсивный подсчет общего веса дерева объектов</p>
                    <input style={styles.input} value={cargoId} onChange={e => setCargoId(e.target.value)} placeholder="Вставьте ID контейнера" />
                    <button style={{...styles.btn, marginTop: '10px'}} onClick={checkComposite}>Анализировать контейнер</button>

                    {cargoData && (
                        <div style={{ marginTop: '15px', padding: '15px', backgroundColor: '#f0fdf4', border: '1px solid #bbf7d0', borderRadius: '8px' }}>
                            <p><b>Название сборки:</b> {cargoData.name}</p>
                            <p style={{ marginTop: '10px' }}><b>Общий брутто-вес:</b> <span style={{ fontSize: '20px', fontWeight: 'bold', color: '#166534' }}>{cargoData.totalWeight} кг</span></p>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
}