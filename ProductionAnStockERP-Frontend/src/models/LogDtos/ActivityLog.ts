export interface ActivityLog {
   id: number;
  userName: string;
  action: string;
  createdAt: string; // ISO string formatında gelecek
  
  // YENİ EKLENEN ALANLAR
  status: 'Başarılı' | 'Başarısız';
  targetEntity?: string;
}
