import React, { useMemo } from 'react';
import { Navbar, Nav, Container, NavDropdown } from 'react-bootstrap';
import  { LinkContainer } from 'react-router-bootstrap';
import { useAuth } from '../contexts/AuthContext';
import { allModules} from '../config/modules';
import type { UserRole } from '../config/modules'; // Merkezi dosyadan import ediyoruz

const AppNavbar: React.FC = () => {
  const { user, logout, loading } = useAuth();

  // HomePage'deki ile aynı filtreleme mantığı
  const visibleModules = useMemo(() => {
    if (!user?.role) return [];
    // Şifre değiştirme modülünü navbar'da göstermeyelim, onu farklı bir yere koyabiliriz.
    return allModules.filter(module => 
      module.path !== '/change-password' && module.roles.includes(user.role as UserRole)
    );
  }, [user?.role]);
  
  // Şifre değiştirme modülünü ayrı alalım
  const changePasswordModule = allModules.find(m => m.path === '/change-password');

  // Kullanıcı bilgisi yüklenirken veya kullanıcı yoksa navbar'ı gösterme
  if (loading || !user) {
    return null; 
  }

  return (
    <Navbar bg="dark" variant="dark" expand="lg" sticky="top" collapseOnSelect>
      <Container fluid="lg">
        <LinkContainer to="/">
          <Navbar.Brand>ERP Sistem</Navbar.Brand>
        </LinkContainer>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto">
            {/* Kullanıcının rolüne göre görünür olan modülleri listele */}
            {visibleModules.map((module) => (
              <LinkContainer to={module.path} key={module.path}>
                <Nav.Link>
                  <i className={`${module.icon} me-2`}></i>{module.title}
                </Nav.Link>
              </LinkContainer>
            ))}
          </Nav>
          <Nav>
            {/* Sağ tarafta kullanıcı bilgisi ve çıkış butonu */}
            <NavDropdown title={<><i className="bi bi-person-circle me-1"></i>{user.userName} ({user.role})</>} id="user-menu-dropdown">
                {changePasswordModule && (
                    <LinkContainer to={changePasswordModule.path}>
                        <NavDropdown.Item>
                           <i className={`${changePasswordModule.icon} me-2`}></i>
                           {changePasswordModule.title}
                        </NavDropdown.Item>
                    </LinkContainer>
                )}
               <NavDropdown.Divider />
               <NavDropdown.Item onClick={logout} className="text-danger">
                 <i className="bi bi-box-arrow-right me-2"></i>
                 Çıkış Yap
               </NavDropdown.Item>
            </NavDropdown>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
};

export default AppNavbar;