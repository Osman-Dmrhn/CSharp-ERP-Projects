export type UserRole = 'Admin' | 'Producer' | 'SalesManager';

export interface Module {
  path: string;
  icon: string;
  title: string;
  description: string;
  roles: UserRole[];
}

export const allModules: Module[] = [
  {
    path: '/orders',
    icon: 'bi-box-seam',
    title: 'Sipariş İşlemleri',
    description: 'Yeni sipariş oluşturun, mevcut siparişleri görüntüleyin ve yönetin.',
    roles:['Admin','SalesManager']
  },
  {
    path: '/production-orders',
    icon: 'bi-tools',
    title: 'Üretim Emirleri',
    description: 'Üretim emirlerini takip edin, planlayın ve tamamlanan işleri raporlayın.',
    roles:['Admin','Producer']
  },
  {
    path: '/stock',
    icon: 'bi-clipboard-data',
    title: 'Stok Hareketleri',
    description: 'Depo stok durumunu, giriş ve çıkış hareketlerini kontrol edin.',
    roles:['Admin','Producer']
  },
  {
    path: '/users',
    icon: 'bi-people',
    title: 'Kullanıcı Yönetimi',
    description: 'Sistem kullanıcılarını ve yetkilerini yönetin.',
    roles:['Admin']
  },
  {
    path: '/products',
    icon: 'bi-tags-fill',
    title: 'Ürün Yönetimi',
    description: 'Sistemdeki ürünleri oluşturun, düzenleyin ve yönetin.',
    roles:['Admin']
  },
  {
    path: '/reports',
    icon: 'bi-file-earmark-bar-graph-fill',
    title: 'Raporlama',
    description: 'Sistem verileriyle ilgili detaylı PDF raporları oluşturun ve indirin.',
    roles: ['Admin', 'SalesManager']
  },
  {
    path: '/logs',
    icon: 'bi-journal-text',
    title: 'Log İşlemleri',
    description: 'Sistemdeki tüm kullanıcı aktivitelerini ve işlem günlüklerini inceleyin.',
    roles:['Admin']
  },
  {
    path: '/change-password',
    icon: 'bi-key-fill', 
    title: 'Şifre Değiştirme',
    description: 'Kullanıcı Şifre Değiştirme.',
    roles:['Admin','Producer','SalesManager']
  }
];