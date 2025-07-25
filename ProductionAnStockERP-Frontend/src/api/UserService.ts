// Dosya: src/api/UserService.ts

import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { User } from '../models/UserDtos/User';
import type { CreateUserDto } from '../models/UserDtos/CreateUserDto';
import type { UpdateUserDto } from '../models/UserDtos/UpdateUserDto';
import type { UpdateUserPasswordDto } from '../models/UserDtos/UpdateUserPasswordDto';
import type { UserFilters } from '../models/UserDtos/UserFilterParameters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

// -------------------- YENİ VERİ YAPI MODELLERİ --------------------

// Backend'den gelen PagedResponse<T> nesnesinin içindeki 'data' kısmının yapısı
interface PaginatedData<T> {
  items: T[];
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

// Backend'den gelen tüm response'un yapısı
interface ApiPaginatedResponse<T> {
  success: boolean;
  message: string;
  data: PaginatedData<T>;
}

// React bileşenine göndereceğimiz nihai yapı
interface ComponentReadyResponse {
  users: User[];
  pagination: PaginationInfo;
}

// -------------------- GÜNCELLENMİŞ SERVİS FONKSİYONLARI --------------------

/**
 * Filtrelenmiş ve sayfalanmış kullanıcı listesini getirir.
 */
export const getAllUsers = async (filters: UserFilters): Promise<ComponentReadyResponse> => {
  const params = new URLSearchParams(filters as any).toString();

  // 1. Backend'den gelen gerçek veri yapısını (ApiPaginatedResponse) bekle
  const response = await api.get<ApiPaginatedResponse<User>>(`/users?${params}`);
  const apiData = response.data;

  // 2. Gelen verinin içindeki 'data' ve 'items' alanlarını doğru şekilde ayrıştır
  if (apiData && apiData.success) {
    const pagedData = apiData.data;
    
    // 3. Sayfalama bilgisini oluştur
    const pagination: PaginationInfo = {
      currentPage: pagedData.currentPage,
      totalPages: pagedData.totalPages,
      pageSize: pagedData.pageSize,
      totalCount: pagedData.totalCount,
      hasPrevious: pagedData.hasPrevious,
      hasNext: pagedData.hasNext,
    };
    
    // 4. React bileşeninin beklediği { users, pagination } nesnesini döndür
    return {
      users: pagedData.items || [],
      pagination: pagination,
    };
  } else {
    // API'den success:false dönerse veya veri bozuksa hata fırlat
    throw new Error(apiData?.message || "Kullanıcılar alınamadı.");
  }
};

// Diğer fonksiyonlar aynı kalabilir, sadece tipleri User olarak güncelledim
export const createUser = async (data: CreateUserDto): Promise<ApiResponse<User>> => {
  const response = await api.post<ApiResponse<User>>('/users', data);
  return response.data;
};

export const updateUser = async (id: number, data: UpdateUserDto): Promise<ApiResponse<User>> => {
  const response = await api.put<ApiResponse<User>>(`/users/${id}`, data);
  return response.data;
};

export const deleteUser = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.delete<ApiResponse<boolean>>(`/users/${id}`);
  return response.data;
};

export const updateUserPassword = async (data: UpdateUserPasswordDto): Promise<ApiResponse<string>> => {
  const response = await api.put<ApiResponse<string>>('/users/password', data);
  return response.data;
};