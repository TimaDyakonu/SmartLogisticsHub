export const API_URL: string = 'https://localhost:7250/api';

export interface Item {
    id: string;
    name: string;
    sku: string;
    weight: number;
    volume: number;
    category: string;
}

export interface Order {
    id: string;
    customerName: string;
    status: string;
    createdAt?: string;
}