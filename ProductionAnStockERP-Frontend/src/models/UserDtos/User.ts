export interface User {
  userId: number;
  userName: string;
  email: string;
  isActive: boolean;
  role: 'Admin' | 'Producer' | 'SalesManager';
}