export interface ActivityLog {
   id: number;
  userName: string;
  action: string;
  createdAt: string; 
  status: 'Başarılı' | 'Başarısız';
  targetEntity?: string;
}

export interface ActivityLogDetail extends ActivityLog {
  targetEntityId?: string;
  changes?: string; 
}
