import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { User } from '../models/UserDtos/User';
import type { CreateUserDto } from '../models/UserDtos/CreateUserDto.ts';
import type { UpdateUserDto } from '../models/UserDtos/UpdateUserDto.ts';
import type { UpdateUserPasswordDto } from '../models/UserDtos/UpdateUserPasswordDto.ts';

export const getAllUsers = async (): Promise<ApiResponse<User[]>> => {
  const response = await api.get<ApiResponse<User[]>>('/users/getalluser');
  return response.data;
};

export const createUser = async (data: CreateUserDto): Promise<ApiResponse<User>> => {
  const response = await api.post<ApiResponse<User>>('/users/createuser', data);
  return response.data;
};

export const updateUser = async (id: number, data: UpdateUserDto): Promise<ApiResponse<User>> => {
  const response = await api.post<ApiResponse<User>>(`/users/updateuser/${id}`, data);
  return response.data;
};

export const deleteUser = async (id: number): Promise<ApiResponse<boolean>> => {
  const response = await api.post<ApiResponse<boolean>>(`/users/deleteuser/${id}`);
  return response.data;
};

export const updateUserPass = async (data: UpdateUserPasswordDto): Promise<ApiResponse<void>> => {
  const response = await api.post('/users/updateuserpassword', data);
  return response.data;
};

