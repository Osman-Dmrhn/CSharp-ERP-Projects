// Dosya: src/api/ReportService.ts

import api from './api';
import type { OrderFilters } from '../models/OrderDtos/OrderFilters ';
import type { ProductFilters } from '../models/ProductDtos/ProductFilters';
import type { ProductionOrderFilters } from '../models/ProductionOrderDtos/ProductionOrderFilters';
import type { StockTransactionFilters } from '../models/StockTransactionDtos/StockTransactionFilters';
import type { LogFilters } from './ActivityLogService';

// Bu yardımcı fonksiyon, bir dosya indirme işlemini tetikler.
const downloadFile = (data: Blob, defaultFileName: string, headers: any) => {
  let fileName = defaultFileName;
  const contentDisposition = headers['content-disposition'];
  if (contentDisposition) {
    const fileNameMatch = contentDisposition.match(/filename="(.+)"/);
    if (fileNameMatch && fileNameMatch.length === 2)
      fileName = fileNameMatch[1];
  }

  const url = window.URL.createObjectURL(new Blob([data]));
  const link = document.createElement('a');
  link.href = url;
  link.setAttribute('download', fileName);
  document.body.appendChild(link);
  link.click();
  link.parentNode?.removeChild(link);
  window.URL.revokeObjectURL(url);
}

// Genel bir rapor oluşturma fonksiyonu
const generateReport = async (endpoint: string, filters: object, defaultFileName: string) => {
  try {
    const params = new URLSearchParams(filters as any).toString();
    const response = await api.get(`/reports/${endpoint}?${params}`, {
      responseType: 'blob', // Axios'a bir dosya beklediğini söylüyoruz
    });
    downloadFile(response.data, defaultFileName, response.headers);
  } catch (error) {
    console.error(`Rapor oluşturulurken hata (${endpoint}):`, error);
    throw new Error(`Filitrelemele ayarlarınızı kontrol ediniz.Rapor oluşturulamadı.`);
  }
};

// Her rapor tipi için ayrı fonksiyonlar
export const generateOrdersReport = (filters: OrderFilters) => 
  generateReport('orders', filters, 'siparis-raporu.pdf');

export const generateProductsReport = (filters: ProductFilters) => 
  generateReport('products', filters, 'urun-raporu.pdf');

export const generateProductionOrdersReport = (filters: ProductionOrderFilters) => 
  generateReport('production-orders', filters, 'uretim-emri-raporu.pdf');

export const generateStockTransactionsReport = (filters: StockTransactionFilters) => 
  generateReport('stock-transactions', filters, 'stok-hareketi-raporu.pdf');

export const generateLogsReport = (filters: LogFilters) => 
  generateReport('logs', filters, 'aktivite-log-raporu.pdf');