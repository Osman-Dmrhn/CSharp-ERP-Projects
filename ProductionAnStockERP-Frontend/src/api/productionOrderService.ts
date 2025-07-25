// Dosya: src/api/ProductionOrderService.ts

import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { ProductionOrder} from '../models/ProductionOrderDtos/ProductionOrder';
import type { ProductionOrderCreateDto } from '../models/ProductionOrderDtos/ProductionOrderCreateDto';
import type { ProductionOrderUpdateDto } from '../models/ProductionOrderDtos/ProductionOrderUpdateDto';
import type { ProductionOrderFilters } from '../models/ProductionOrderDtos/ProductionOrderFilters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

interface PaginatedProductionOrderResponse {
  productionOrders: ProductionOrder[];
  pagination: PaginationInfo;
}

export const getAllProductionOrders = async (filters: ProductionOrderFilters): Promise<PaginatedProductionOrderResponse> => {
  const params = new URLSearchParams(filters as any).toString();
  const response = await api.get<ProductionOrder[]>(`/productionorders?${params}`);
  
  const paginationHeader = response.headers['x-pagination'];
  const pagination: PaginationInfo = paginationHeader ? JSON.parse(paginationHeader) : {} as PaginationInfo;

  return { productionOrders: response.data, pagination };
};

export const createProductionOrder = async (data: ProductionOrderCreateDto): Promise<ApiResponse<ProductionOrder>> => {
  const response = await api.post<ApiResponse<ProductionOrder>>('/productionorders', data);
  return response.data;
};

export const updateProductionOrder = async (id: number, data: ProductionOrderUpdateDto): Promise<ApiResponse<ProductionOrder>> => {
  const response = await api.put<ApiResponse<ProductionOrder>>(`/productionorders/${id}`, data);
  return response.data;
};

export const deleteProductionOrder = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.delete<ApiResponse<boolean>>(`/productionorders/${id}`);
  return response.data;
};