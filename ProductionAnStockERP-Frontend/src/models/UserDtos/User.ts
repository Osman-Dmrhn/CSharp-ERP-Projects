export interface User {
  id: string;
  userName: string;
  email: string;
  role: 'Admin' | 'User' | 'Editor'; // Rolleri projenize göre düzenleyin
}