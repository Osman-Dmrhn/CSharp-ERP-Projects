import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { ActivityLog } from '../models/LogDtos/ActivityLog';
import type {PaginationInfo} from '../models/LogDtos/PaginationInfo';
import type { ActivityLogDetail } from '../models/LogDtos/ActivityLog';

export interface LogFilters {
  pageNumber?: number;
  pageSize?: number;
  startDate?: string;
  endDate?: string;
  userName?: string;
  status?: string;
  searchTerm?: string;
}


// API'den dönen cevabın yapısı
interface PaginatedLogResponse {
  logs: ActivityLog[];
  pagination: PaginationInfo;
}

export const getAllActivityLogs = async (filters: LogFilters): Promise<PaginatedLogResponse> => {
  // Filtre nesnesini URL query string'e çeviriyoruz
  // Boş olan filtreleri sorguya eklememek için temizliyoruz
  const cleanedFilters: Record<string, any> = {};
  for (const key in filters) {
    if (Object.prototype.hasOwnProperty.call(filters, key)) {
      const value = filters[key as keyof LogFilters];
      if (value) { // Sadece dolu olanları ekle
        cleanedFilters[key] = value;
      }
    }
  }
  const params = new URLSearchParams(cleanedFilters);

  const response = await api.get<ActivityLog[]>(`/activitylog?${params.toString()}`);
  
  const paginationHeader = response.headers['x-pagination'];
  const pagination: PaginationInfo = paginationHeader ? JSON.parse(paginationHeader) : {} as PaginationInfo;

  return {
    logs: response.data,
    pagination: pagination,
  };
};

export const getLogsByUserId = async (id: number): Promise<ApiResponse<ActivityLog[]>> => {
  const response = await api.get<ApiResponse<ActivityLog[]>>(`/activitylog/getlogsbyuserid/${id}`);
  return response.data;
};

export const getLogById = async (id: number): Promise<ApiResponse<ActivityLog>> => {
  const response = await api.get<ApiResponse<ActivityLog>>(`/activitylog/getlogbyid/${id}`);
  return response.data;
};

export const getLogDetails = async (id: number): Promise<ApiResponse<ActivityLogDetail>> => {

  const response = await api.get<ApiResponse<ActivityLogDetail>>(`/activitylog/${id}`);
  return response.data;
};
