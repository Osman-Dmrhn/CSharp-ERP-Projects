
import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { Order } from '../models/OrderDtos/Order';
import type { OrderUpdateDto } from '../models/OrderDtos/OrderUpdate';
import type { CreateOrderDto } from '../models/OrderDtos/CreateOrderDto';
import type { OrderFilters } from '../models/OrderDtos/OrderFilters ';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

interface PaginatedOrderResponse {
  orders: Order[];
  pagination: PaginationInfo;
}

export const getAllOrders = async (filters: OrderFilters): Promise<PaginatedOrderResponse> => {
  const params = new URLSearchParams(filters as any).toString();
  const response = await api.get<Order[]>(`/orders?${params}`);
  
  const paginationHeader = response.headers['x-pagination'];
  const pagination: PaginationInfo = paginationHeader ? JSON.parse(paginationHeader) : {} as PaginationInfo;

  return { orders: response.data, pagination };
};

export const getOrderById = async (id: number): Promise<ApiResponse<Order>> => {
  const response = await api.get<ApiResponse<Order>>(`/orders/${id}`);
  return response.data;
};

export const createOrder = async (orderData: CreateOrderDto): Promise<ApiResponse<Order>> => {
  const response = await api.post<ApiResponse<Order>>('/orders', orderData);
  return response.data;
};

export const updateOrder = async (id: number, orderData: OrderUpdateDto): Promise<ApiResponse<Order>> => {
  const response = await api.put<ApiResponse<Order>>(`/orders/${id}`, orderData);
  return response.data;
};

export const deleteOrder = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.delete<ApiResponse<boolean>>(`/orders/${id}`);
  return response.data;
};