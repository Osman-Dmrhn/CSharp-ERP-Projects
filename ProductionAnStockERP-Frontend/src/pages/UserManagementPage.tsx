// Dosya: src/pages/UserManagementPage.tsx

import React, { useEffect, useState, useCallback } from 'react';
import { getAllUsers, createUser, updateUser, deleteUser } from '../api/UserService';
import type { User } from '../models/UserDtos/User';
import type { CreateUserDto } from '../models/UserDtos/CreateUserDto';
import type { UpdateUserDto } from '../models/UserDtos/UpdateUserDto';
import type { UserFilters } from '../models/UserDtos/UserFilterParameters';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { PaginationComponent } from '../components/Pagination';
import { Spinner, Alert, Table, Badge, Button, Modal, Form, Row, Col, Container } from 'react-bootstrap';

const emptyCreateUser: CreateUserDto = { userName: '', email: '', passwordHash: '', role: 'Producer' };

interface EditUserModalProps {
  user: User;
  onSave: (data: UpdateUserDto) => void;
  onHide: () => void;
}

const EditUserModal: React.FC<EditUserModalProps> = ({ user, onSave, onHide }) => {
  const [formData, setFormData] = useState<UpdateUserDto>({
    userName: user.userName,
    email: user.email,
    role: user.role,
    isActive: user.isActive,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = e.target;
    const isCheckbox = type === 'checkbox';
    const checkedValue = isCheckbox ? (e.target as HTMLInputElement).checked : undefined;
    setFormData(prev => ({ ...prev, [name]: isCheckbox ? checkedValue : value }));
  };

  const handleSubmit = () => { onSave(formData); };

  return (
    <Modal show onHide={onHide} centered>
      <Modal.Header closeButton><Modal.Title>Kullanıcı Güncelle</Modal.Title></Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Ad</Form.Label>
            <Form.Control type="text" name="userName" value={formData.userName} onChange={handleChange} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Email</Form.Label>
            <Form.Control type="email" name="email" value={formData.email} onChange={handleChange} />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Rol</Form.Label>
            <Form.Select name="role" value={formData.role} onChange={handleChange}>
              <option value="Admin">Admin</option>
              <option value="Producer">Producer</option>
              <option value="SalesManager">SalesManager</option>
            </Form.Select>
          </Form.Group>
          <Form.Check type="switch" id="is-active-switch" name="isActive" label="Aktif mi?" checked={formData.isActive} onChange={handleChange} />
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>İptal</Button>
        <Button variant="primary" onClick={handleSubmit}>Kaydet</Button>
      </Modal.Footer>
    </Modal>
  );
};

const UserManagementPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<UserFilters>({ pageNumber: 1, pageSize: 10 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [newUser, setNewUser] = useState<CreateUserDto>(emptyCreateUser);
  const [editingUser, setEditingUser] = useState<User | null>(null);

  const fetchUsers = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      // 1. Servisten { users, pagination } nesnesini al
      const result = await getAllUsers(filters);
      // 2. State'e bu nesnenin içindeki 'users' dizisini ata
      setUsers(result.users || []); 
      setPagination(result.pagination);
    } catch (err) {
      setError('Kullanıcılar alınırken bir hata oluştu.');
      setUsers([]); // Hata durumunda state'i boş dizi yap
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchUsers();
  }, [fetchUsers]);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    const result = await createUser(newUser);
    if (result.success) {
      fetchUsers(); // Listeyi yenile
      setNewUser(emptyCreateUser);
    } else {
      setError(result.message);
    }
  };

  const handleDelete = async (id: number) => {
    if (window.confirm("Bu kullanıcıyı silmek istediğinizden emin misiniz?")) {
      const result = await deleteUser(id);
      if (result.success) {
        if (users.length === 1 && pagination && pagination.currentPage > 1) {
            handlePageChange(pagination.currentPage - 1);
        } else {
            fetchUsers();
        }
      } else {
        setError(result.message);
      }
    }
  };

  const handleUpdate = async (updateData: UpdateUserDto) => {
    if (editingUser) {
      const result = await updateUser(editingUser.userId, updateData);
      if (result.success) {
        fetchUsers();
        setEditingUser(null);
      } else {
        setError(result.message);
      }
    }
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({ ...prev, pageNumber: page }));
  };

  return (
    <Container fluid className="p-4">
      <div className="bg-white rounded shadow p-4 mb-4">
        <h2 className="text-primary mb-4">Yeni Kullanıcı Oluştur</h2>
        <Form onSubmit={handleCreate}>
          <Row className="g-3">
            <Col md={3}><Form.Control type="text" placeholder="Ad Soyad" value={newUser.userName} onChange={(e) => setNewUser({ ...newUser, userName: e.target.value })} required /></Col>
            <Col md={3}><Form.Control type="email" placeholder="Email" value={newUser.email} onChange={(e) => setNewUser({ ...newUser, email: e.target.value })} required /></Col>
            <Col md={2}><Form.Control type="password" placeholder="Şifre" value={newUser.passwordHash} onChange={(e) => setNewUser({ ...newUser, passwordHash: e.target.value })} required /></Col>
            <Col md={2}>
              <Form.Select value={newUser.role} onChange={(e) => setNewUser({ ...newUser, role: e.target.value as CreateUserDto['role'] })}>
                <option value="Producer">Producer</option>
                <option value="Admin">Admin</option>
                <option value="SalesManager">SalesManager</option>
              </Form.Select>
            </Col>
            <Col md={2}><Button type="submit" className="w-100">Ekle</Button></Col>
          </Row>
        </Form>
      </div>
      <div className="bg-white rounded shadow p-4">
        <h3 className="mb-3">Kullanıcı Listesi</h3>
        {loading ? <div className="text-center"><Spinner animation="border" /></div> : error ? <Alert variant="danger">{error}</Alert> : (
          <>
            <Table striped bordered hover responsive>
              <thead className="table-dark">
                <tr>
                  <th>Ad Soyad</th><th>Email</th><th>Rol</th><th>Durum</th><th className="text-center">İşlemler</th>
                </tr>
              </thead>
              <tbody>
                {users.map((user) => (
                  <tr key={user.userId}>
                    <td>{user.userName}</td>
                    <td>{user.email}</td>
                    <td>{user.role}</td>
                    <td><Badge bg={user.isActive ? 'success' : 'secondary'}>{user.isActive ? 'Aktif' : 'Pasif'}</Badge></td>
                    <td className="text-center">
                      <Button variant="warning" size="sm" className="me-2" onClick={() => setEditingUser(user)}>Düzenle</Button>
                      <Button variant="danger" size="sm" onClick={() => handleDelete(user.userId)}>Sil</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
            <div className="d-flex justify-content-end">
              {pagination && <PaginationComponent pagination={pagination} onPageChange={handlePageChange} />}
            </div>
          </>
        )}
      </div>
      {editingUser && <EditUserModal user={editingUser} onSave={handleUpdate} onHide={() => setEditingUser(null)} />}
    </Container>
  );
};

export default UserManagementPage;