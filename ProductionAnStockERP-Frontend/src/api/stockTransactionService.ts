import api from './api'; // Axios instance importu
import type { StockTransaction } from '../models/StockTransactionDtos/StockTransaction';
import type { StockTransactionCreateDto } from '../models/StockTransactionDtos/StockTransactionCreateDto';
import type { StockTransactionUpdateDto } from '../models/StockTransactionDtos/StockTransactionUpdateDto';
import type { ApiResponse } from '../models/ApiResponse';

// Stok hareketi servisi
const stockTransactionService = {
  // Tüm stok hareketlerini al
  getAllStockTransactions: async (): Promise<ApiResponse<StockTransaction[]>> => {
    try {
      const response = await api.get('/stocktransaction/getallstocktransaction');
      return response.data; // API'den gelen cevabı döndürüyoruz
    } catch (error) {
      throw error;
    }
  },

  // Belirli bir stok hareketini al
  getStockTransactionById: async (id: number): Promise<ApiResponse<StockTransaction>> => {
    try {
      const response = await api.get(`/stocktransaction/getstocktransaction/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Yeni bir stok hareketi oluştur
  createStockTransaction: async (data: StockTransactionCreateDto): Promise<ApiResponse<StockTransaction>> => {
    try {
      const response = await api.post('/stocktransaction/createstocktransaction', data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Mevcut stok hareketini güncelle
  updateStockTransaction: async (id: number, data: StockTransactionUpdateDto): Promise<ApiResponse<StockTransaction>> => {
    try {
      const response = await api.post(`/stocktransaction/updatestocktransaction/${id}`, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Stok hareketini sil
  deleteStockTransaction: async (id: number): Promise<ApiResponse<void>> => {
    try {
      const response = await api.post(`/stocktransaction/deletestocktransaction/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }
};

export default stockTransactionService;
