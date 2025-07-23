import api from './api';
import type { ApiResponse } from '../models/ApiResponse';
import type { ActivityLog } from '../models/LogDtos/ActivityLog';

export const getAllActivityLogs = async (): Promise<ApiResponse<ActivityLog[]>> => {
  const response = await api.get<ApiResponse<ActivityLog[]>>('/activitylog/getallactivitiylogs');
  return response.data;
};

export const getLogsByUserId = async (id: number): Promise<ApiResponse<ActivityLog[]>> => {
  const response = await api.get<ApiResponse<ActivityLog[]>>(`/activitylog/getlogsbyuserid/${id}`);
  return response.data;
};

export const getLogById = async (id: number): Promise<ApiResponse<ActivityLog>> => {
  const response = await api.get<ApiResponse<ActivityLog>>(`/activitylog/getlogbyid/${id}`);
  return response.data;
};
