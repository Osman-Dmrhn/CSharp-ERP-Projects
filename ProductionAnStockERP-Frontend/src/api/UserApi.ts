import api from "./api"
import type { ApiResponse } from "../models/ApiResponse";
import type {User} from "../models/UserDtos/User"
// Backend'den gelecek kullanıcı verisinin tipini tanımlayalım


// /me endpoint'ine istek atacak fonksiyon
export const getMe = async (): Promise<User> => {
  const response = await api.get<ApiResponse<User>>('/users/me'); // Endpoint yolunu kendinize göre güncelleyin
  return response.data.data;
};