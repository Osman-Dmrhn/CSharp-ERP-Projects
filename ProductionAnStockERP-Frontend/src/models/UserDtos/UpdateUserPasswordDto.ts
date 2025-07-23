export interface UpdateUserPasswordDto {
  userId: number;
  oldpass: string;
  newpass: string;
}