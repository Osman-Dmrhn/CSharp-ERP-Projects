import React, { useMemo } from 'react';
import { Container, Row, Col, Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import '../assets/css/HomePage.css'; 
import { useAuth } from "../contexts/AuthContext";

type UserRole = 'Admin' | 'Producer' | 'SalesManager';

interface Module {
  path: string;
  icon: string;
  title: string;
  description: string;
  roles: UserRole[];
}

const allModules: Module[] = [
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
    path: '/activity-log',
    icon: 'bi-journal-text',
    title: 'Log İşlemleri',
    description: 'Sistemdeki tüm kullanıcı aktivitelerini ve işlem günlüklerini inceleyin.',
    roles:['Admin']
  },
  {
    path: '/change-password', // Path'i de değiştirmek daha anlamlı olabilir
    // 2. İKON GÜNCELLEMESİ
    icon: 'bi-key-fill', 
    title: 'Şifre Değiştirme',
    description: 'Kullanıcı Şifre Değiştirme.',
    roles:['Admin','Producer','SalesManager']
  }   
];

const HomePage: React.FC = () => {
    // 1. ÇIKIŞ BUTONU İÇİN `logout` FONKSİYONUNU AL
    const { user, logout } = useAuth();
    
    const visibleModules = useMemo(() => {
    return allModules.filter(module => 
      user?.role && module.roles.includes(user.role as UserRole)
    );
  }, [user?.role]);

  return (
    <Container fluid="lg" className="mt-4">
      <div className="p-4 mb-4 bg-light rounded-3">
        <Container fluid className="py-3">
            {/* 1. ÇIKIŞ BUTONU EKLENDİ VE BAŞLIK ALANI DÜZENLENDİ */}
            <div className="d-flex justify-content-between align-items-center">
                <h1 className="display-5 fw-bold">Hoş Geldiniz, {user?.userName}!</h1>
                <Button variant="outline-danger" onClick={logout}>
                    <i className="bi bi-box-arrow-right me-2"></i>
                    Çıkış Yap
                </Button>
            </div>
            <p className="col-md-8 fs-4 mt-2">
              ERP sistemine hoş geldiniz. İşlemlerinize başlamak için aşağıdaki modüllerden birini seçebilirsiniz.
            </p>
        </Container>
      </div>

      {/* 3. KARTLARI ORTALAMAK İÇİN `justify-content-center` EKLENDİ */}
      <Row xs={1} md={2} lg={3} className="g-4 justify-content-center">
        {visibleModules.map((module: Module) => (
          <Col key={module.path}>
            <Card className="h-100 shadow-sm module-card">
              <Card.Body className="d-flex flex-column">
                <div className="d-flex align-items-start mb-3">
                  <i className={`${module.icon} h1 me-3 text-primary`}></i>
                  <Card.Title as="h4" className="mb-0">{module.title}</Card.Title>
                </div>
                <Card.Text className="flex-grow-1">
                  {module.description}
                </Card.Text>
                <Link to={module.path} className="mt-auto">
                  <Button variant="primary" className="w-100">
                    Modüle Git <i className="bi bi-arrow-right-short"></i>
                  </Button>
                </Link>
              </Card.Body>
            </Card>
          </Col>
        ))}
      </Row>
    </Container>
  );
};

export default HomePage;