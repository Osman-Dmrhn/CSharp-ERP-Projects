import api from './api';  // api.ts dosyasını import ediyoruz
import type { Order} from '../models/OrderDtos/Order';
import type {OrderUpdateDto} from '../models/OrderDtos/OrderUpdate'
import type  { CreateOrderDto } from '../models/OrderDtos/CreateOrderDto';
import type { ApiResponse } from '../models/ApiResponse';

// Sipariş servisimiz
const orderService = {
  // Siparişleri al
  getAllOrders: async (): Promise<ApiResponse<Order[]>> => {
    try {
      const response = await api.get('/orders/getallorders'); // Axios instance kullanılıyor
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Sipariş ID'si ile siparişi al
  getOrderById: async (id: number): Promise<ApiResponse<Order>> => {
    try {
      const response = await api.get(`/orders/getorderbyid?id=${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Yeni sipariş oluştur
  createOrder: async (orderData: CreateOrderDto): Promise<ApiResponse<Order>> => {
    try {
      const response = await api.post('/orders/createorder', orderData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Siparişi güncelle
  updateOrder: async (id: number, orderData: OrderUpdateDto): Promise<ApiResponse<Order>> => {
    try {
      const response = await api.post(`/orders/updateorder/${id}`, orderData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Siparişi sil
  deleteOrder: async (id: number): Promise<ApiResponse<void>> => {
    try {
      const response = await api.post(`/orders/deleteorder/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }
};

export default orderService;
