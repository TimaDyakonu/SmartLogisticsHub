import { useState, useEffect } from 'react';
import { API_URL } from '../config';
import { styles } from '../styles';

export default function DashboardPage() {
    const [config, setConfig] = useState<any>(null);
    const [report, setReport] = useState<any>(null);

    useEffect(() => {
        fetch(`${API_URL}/inventory/config`).then(res => res.json()).then(setConfig);
        fetch(`${API_URL}/report/inventory-summary`).then(res => res.json()).then(setReport);
    },[]);

    return (
        <div>
            <h1 style={styles.header}>Обзор логистического центра</h1>

            <div style={{ display: 'flex', gap: '20px' }}>
                <div style={{ ...styles.card, flex: 1 }}>
                    <h3>Системная конфигурация</h3>
                    <span style={styles.badge}>Singleton Pattern</span>
                    {config ? (
                        <ul style={{ marginTop: '15px', lineHeight: '2' }}>
                            <li><b>Код хаба:</b> {config.hubCode}</li>
                            <li><b>Валюта:</b> {config.operatingCurrency}</li>
                            <li><b>Налоговая ставка:</b> {config.taxRate * 100}%</li>
                        </ul>
                    ) : <p>Загрузка...</p>}
                </div>

                <div style={{ ...styles.card, flex: 2 }}>
                    <h3>Сводный отчет по складу</h3>
                    <span style={styles.badge}>Builder Pattern</span>
                    {report ? (
                        <div style={{ marginTop: '15px' }}>
                            <p><b>Статус:</b> {report.summary}</p>
                            <div style={styles.logBox}>
                                {report.sections?.map((sec: string, i: number) => <div key={i}>&gt; {sec}</div>)}
                            </div>
                        </div>
                    ) : <p>Загрузка...</p>}
                </div>
            </div>
        </div>
    );
}