import api from './api'; // api.ts dosyasını import ediyoruz
import type {ProductionOrderCreateDto} from '../models/ProductionOrderDtos/ProductionOrderCreateDto';
import type { ProductionOrder } from '../models/ProductionOrderDtos/ProductionOrder';
import type { ProductionOrderUpdateDto } from '../models/ProductionOrderDtos/ProductionOrderUpdateDto';
import type { ApiResponse } from '../models/ApiResponse';

// Production Order servisimiz
const productionOrderService = {
  // Üretim siparişlerini al
  getAllProductionOrders: async (): Promise<ApiResponse<ProductionOrder[]>> => {
    try {
      const response = await api.get('/productionorders/GetAllProductionOrders');
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Üretim siparişi ID'si ile siparişi al
  getProductionOrderById: async (id: number): Promise<ApiResponse<ProductionOrder>> => {
    try {
      const response = await api.get(`/productionorders/getproductionorderbyid/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Yeni üretim siparişi oluştur
  createProductionOrder: async (orderData: ProductionOrderCreateDto): Promise<ApiResponse<ProductionOrder>> => {
    try {
      const response = await api.post('/productionorders/createproductionorder', orderData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Üretim siparişi güncelle
  updateProductionOrder: async (id: number, orderData: ProductionOrderUpdateDto): Promise<ApiResponse<ProductionOrder>> => {
    try {
      const response = await api.post(`/productionorders/updateproductionorder/${id}`, orderData);
      return response.data;
    } catch (error) {
      throw error;
    }
  },

  // Üretim siparişi sil
  deleteProductionOrder: async (id: number): Promise<ApiResponse<void>> => {
    try {
      const response = await api.post(`/productionorders/deleteproductionorder/${id}`);
      return response.data;
    } catch (error) {
      throw error;
    }
  }
};

export default productionOrderService;
