// Dosya: src/api/StockTransactionService.ts

import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { StockTransaction} from '../models/StockTransactionDtos/StockTransaction';
import type { StockTransactionCreateDto } from '../models/StockTransactionDtos/StockTransactionCreateDto';
import type { StockTransactionUpdateDto } from '../models/StockTransactionDtos/StockTransactionUpdateDto';
import type { StockTransactionFilters } from '../models/StockTransactionDtos/StockTransactionFilters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

interface PaginatedStockTransactionResponse {
  stockTransactions: StockTransaction[];
  pagination: PaginationInfo;
}

export const getAllStockTransactions = async (filters: StockTransactionFilters): Promise<PaginatedStockTransactionResponse> => {
  const params = new URLSearchParams(filters as any).toString();
  const response = await api.get<StockTransaction[]>(`/stocktransactions?${params}`);
  
  const paginationHeader = response.headers['x-pagination'];
  const pagination: PaginationInfo = paginationHeader ? JSON.parse(paginationHeader) : {} as PaginationInfo;

  return { stockTransactions: response.data, pagination };
};

export const createStockTransaction = async (data: StockTransactionCreateDto): Promise<ApiResponse<StockTransaction>> => {
  const response = await api.post<ApiResponse<StockTransaction>>('/stocktransactions', data);
  return response.data;
};

export const updateStockTransaction = async (id: number, data: StockTransactionUpdateDto): Promise<ApiResponse<StockTransaction>> => {
  const response = await api.put<ApiResponse<StockTransaction>>(`/stocktransactions/${id}`, data);
  return response.data;
};

export const deleteStockTransaction = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.delete<ApiResponse<boolean>>(`/stocktransactions/${id}`);
  return response.data;
};