import api from './api';
import type { UserLoginRequest } from '../models/UserLoginRequest';
import type { ApiResponse } from '../models/apiResponse';

class AuthService {
  async login(requestData: UserLoginRequest): Promise<ApiResponse<string>> {
    try {
      const apiRequestBody = {
        Email: requestData.email,
        PasswordHash: requestData.passwordHash,
      };

      console.log('API\'ye gönderilen istek gövdesi:', apiRequestBody);

      const response = await api.post<ApiResponse<string>>('/users/login', apiRequestBody);

      return response.data;

    } catch (error) {
      console.error('Login işlemi sırasında hata oluştu:', error);
      throw error;
    }
  }
}

const authService = new AuthService();
export default authService;