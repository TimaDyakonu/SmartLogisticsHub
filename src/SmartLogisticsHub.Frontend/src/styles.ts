import {type CSSProperties} from 'react';

export const styles: Record<string, CSSProperties> = {
    container: {
        display: 'flex',
        minHeight: '100vh',
        fontFamily: 'Segoe UI, Tahoma, Geneva, Verdana, sans-serif',
        backgroundColor: '#f1f5f9',
        color: '#334155'
    },
    sidebar: {
        width: '260px',
        backgroundColor: '#0f172a',
        color: 'white',
        padding: '20px',
        display: 'flex',
        flexDirection: 'column'
    },
    logo: {
        color: '#38bdf8',
        fontSize: '24px',
        fontWeight: 'bold',
        marginBottom: '30px',
        borderBottom: '1px solid #334155',
        paddingBottom: '15px'
    },
    link: {
        color: '#cbd5e1',
        textDecoration: 'none',
        margin: '12px 0',
        fontSize: '15px',
        fontWeight: 500,
        display: 'flex',
        alignItems: 'center',
        gap: '10px'
    },
    content: {flex: 1, padding: '40px', overflowY: 'auto'},
    header: {fontSize: '28px', fontWeight: 600, marginBottom: '25px', color: '#0f172a'},
    card: {
        backgroundColor: 'white',
        padding: '25px',
        borderRadius: '10px',
        boxShadow: '0 1px 3px 0 rgb(0 0 0 / 0.1)',
        marginBottom: '25px'
    },
    table: {width: '100%', borderCollapse: 'collapse', marginTop: '15px'},
    th: {textAlign: 'left', padding: '12px', borderBottom: '2px solid #e2e8f0', color: '#64748b', fontWeight: 600},
    td: {padding: '12px', borderBottom: '1px solid #e2e8f0', color: '#334155'},
    btn: {
        backgroundColor: '#2563eb',
        color: 'white',
        border: 'none',
        padding: '10px 18px',
        borderRadius: '6px',
        cursor: 'pointer',
        fontWeight: 500,
        transition: '0.2s'
    },
    btnOutline: {
        backgroundColor: 'transparent',
        color: '#2563eb',
        border: '1px solid #2563eb',
        padding: '8px 14px',
        borderRadius: '6px',
        cursor: 'pointer',
        fontWeight: 500
    },
    btnDanger: {
        backgroundColor: '#ef4444',
        color: 'white',
        border: 'none',
        padding: '10px 18px',
        borderRadius: '6px',
        cursor: 'pointer',
        fontWeight: 500
    },
    input: {padding: '10px', borderRadius: '6px', border: '1px solid #cbd5e1', width: '100%', boxSizing: 'border-box'},
    formGroup: {marginBottom: '15px'},
    badge: {
        padding: '4px 8px',
        borderRadius: '12px',
        fontSize: '12px',
        fontWeight: 600,
        backgroundColor: '#e0f2fe',
        color: '#0284c7'
    },
    logBox: {
        backgroundColor: '#1e293b',
        color: '#a7f3d0',
        padding: '15px',
        borderRadius: '8px',
        overflowX: 'auto',
        fontSize: '13px',
        fontFamily: 'monospace',
        marginTop: '10px'
    }
};