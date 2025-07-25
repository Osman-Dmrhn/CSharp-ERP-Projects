// Dosya: src/api/ProductService.ts

import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { ProductDto } from '../models/ProductDtos/ProductDto';
import type { ProductCreateDto } from '../models/ProductDtos/ProductCreateDto';
import type { ProductUpdateDto } from '../models/ProductDtos/ProductUpdateDto';
import type { ProductFilters } from '../models/ProductDtos/ProductFilters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

interface PaginatedProductResponse {
  products: ProductDto[];
  pagination: PaginationInfo;
}

export const getAllProducts = async (filters: ProductFilters): Promise<PaginatedProductResponse> => {
  const params = new URLSearchParams(filters as any).toString();
  const response = await api.get<ProductDto[]>(`/products?${params}`);
  
  const paginationHeader = response.headers['x-pagination'];
  const pagination: PaginationInfo = paginationHeader ? JSON.parse(paginationHeader) : {} as PaginationInfo;

  return { products: response.data, pagination };
};

export const createProduct = async (data: ProductCreateDto): Promise<ApiResponse<ProductDto>> => {
  const response = await api.post<ApiResponse<ProductDto>>('/products', data);
  return response.data;
};

export const updateProduct = async (id: number, data: ProductUpdateDto): Promise<ApiResponse<ProductDto>> => {
  const response = await api.put<ApiResponse<ProductDto>>(`/products/${id}`, data);
  return response.data;
};

export const deleteProduct = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.delete<ApiResponse<boolean>>(`/products/${id}`);
  return response.data;
};