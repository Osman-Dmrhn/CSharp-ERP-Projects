export interface UpdateUserDto {
  userName: string;
  email: string;
  isActive: boolean;
  role: 'Admin' | 'Producer' | 'SalesManager';
}
