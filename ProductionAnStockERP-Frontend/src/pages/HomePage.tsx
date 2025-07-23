import React, { useMemo } from 'react';
import { Container, Row, Col, Card, Button } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import '../assets/css/HomePage.css'; 
import { useAuth } from "../contexts/AuthContext";
import { allModules } from '../config/modules';
import type { UserRole, Module } from '../config/modules'; 
import FullPageSpinner from './FullPageSpinner';

const HomePage: React.FC = () => {
  const { user, loading } = useAuth();
  if (loading || !user) {
    return (
      <FullPageSpinner/>
    );
  }

  const visibleModules = useMemo(() => {
    return allModules.filter(module => 
      user?.role && module.roles.includes(user.role as UserRole)
    );
  }, [user?.role]);

  return (   
    <Container fluid="lg" className="py-5">
      <div className="text-center mb-5">
        <h2 className="fw-bold display-6">Kontrol Paneli</h2>
        <p className="lead text-muted">
          Rolünüze tanımlı modüllere buradan hızlıca erişebilirsiniz.
        </p>
      </div>
      <Row xs={1} md={2} lg={3} className="g-4 justify-content-center">
        {visibleModules.map((module: Module) => (
          <Col key={module.path}>
            <Card className="h-100 shadow-sm module-card">
              <Card.Body className="d-flex flex-column text-center">
                <i className={`${module.icon} h1 mb-3 text-primary`}></i>
                <Card.Title as="h5" className="fw-bold mb-2">{module.title}</Card.Title>
                <Card.Text className="text-muted flex-grow-1">
                  {module.description}
                </Card.Text>
                <Link to={module.path} className="mt-auto">
                  <Button variant="outline-primary" className="w-100">
                    Modüle Git
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