export interface CreateUserDto {
  userName: string;
  email: string;
  passwordHash: string;
  role: 'Admin' | 'Producer' | 'SalesManager';
}
