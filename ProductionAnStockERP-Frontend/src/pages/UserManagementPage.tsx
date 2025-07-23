import React, { useEffect, useState } from 'react';
import {
  getAllUsers,
  createUser,
  updateUser,
  deleteUser,
} from '../api/UserService';
import type { User } from '../models/UserDtos/User';
import type { CreateUserDto } from '../models/UserDtos/CreateUserDto';
import type { UpdateUserDto } from '../models/UserDtos/UpdateUserDto';

const emptyCreateUser: CreateUserDto = {
  userName: '',
  email: '',
  passwordHash: '',
  role: 'Admin',
};

const UserManagementPage: React.FC = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [newUser, setNewUser] = useState<CreateUserDto>(emptyCreateUser);
  const [editUserId, setEditUserId] = useState<number | null>(null);
  const [editFormData, setEditFormData] = useState<UpdateUserDto>({
    userName: '',
    email: '',
    role: 'Admin',
    isActive: true,
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    const result = await getAllUsers();
    if (result.success) {
      setUsers(result.data);
    }
    setLoading(false);
  };

  const handleCreate = async () => {
    const result = await createUser(newUser);
    if (result.success) {
      fetchUsers();
      setNewUser(emptyCreateUser);
    }
  };

  const handleDelete = async (id: number) => {
    const result = await deleteUser(id);
    if (result.success) {
      fetchUsers();
    }
  };

  const handleUpdate = async () => {
    if (editUserId !== null && !isNaN(editUserId)) {
      const result = await updateUser(editUserId, editFormData);
      if (result.success) {
        fetchUsers();
        setEditUserId(null);
      }
    }
  };

  const handleEditClick = (user: User) => {
    setEditUserId(user.userId);
    setEditFormData({
      userName: user.userName,
      email: user.email,
      role: user.role,
      isActive: user.isActive,
    });
  };

  return (
    <div className="container-fluid min-h-screen bg-light py-5 px-3">
      <div className="row justify-content-center">
        <div className="col-lg-10 col-xl-9">
          <div className="bg-white rounded shadow p-4 mb-5">
            <h2 className="text-primary mb-4">Yeni Kullanıcı Oluştur</h2>
            <div className="row g-3">
              <div className="col-md-3">
                <input
                  type="text"
                  placeholder="Ad"
                  value={newUser.userName}
                  onChange={(e) =>
                    setNewUser({ ...newUser, userName: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-3">
                <input
                  type="email"
                  placeholder="Email"
                  value={newUser.email}
                  onChange={(e) =>
                    setNewUser({ ...newUser, email: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-3">
                <input
                  type="password"
                  placeholder="Şifre"
                  value={newUser.passwordHash}
                  onChange={(e) =>
                    setNewUser({ ...newUser, passwordHash: e.target.value })
                  }
                  className="form-control"
                />
              </div>
              <div className="col-md-3">
                <select
                  value={newUser.role}
                  onChange={(e) =>
                    setNewUser({
                      ...newUser,
                      role: e.target.value as CreateUserDto['role'],
                    })
                  }
                  className="form-select"
                >
                  <option value="Admin">Admin</option>
                  <option value="Producer">Producer</option>
                  <option value="SalesManager">SalesManager</option>
                </select>
              </div>
            </div>
            <div className="mt-3 text-end">
              <button
                onClick={handleCreate}
                className="btn btn-primary"
              >
                Ekle
              </button>
            </div>
          </div>

          <div className="bg-white rounded shadow p-4">
            <h3 className="text-dark mb-3">Kullanıcı Listesi</h3>
            {loading ? (
              <p>Yükleniyor...</p>
            ) : (
              <div className="table-responsive">
                <table className="table table-bordered table-hover">
                  <thead className="table-light">
                    <tr>
                      <th>Ad</th>
                      <th>Email</th>
                      <th>Rol</th>
                      <th>Durum</th>
                      <th className="text-center">İşlemler</th>
                    </tr>
                  </thead>
                  <tbody>
                    {users.map((user) => (
                      <tr key={user.userId}>
                        <td>{user.userName}</td>
                        <td>{user.email}</td>
                        <td>{user.role}</td>
                        <td>
                          <span
                            className={`badge ${
                              user.isActive ? 'bg-success' : 'bg-secondary'
                            }`}
                          >
                            {user.isActive ? 'Aktif' : 'Pasif'}
                          </span>
                        </td>
                        <td className="text-center">
                          <button
                            className="btn btn-warning btn-sm me-2"
                            onClick={() => handleEditClick(user)}
                          >
                            Güncelle
                          </button>
                          <button
                            className="btn btn-danger btn-sm"
                            onClick={() => handleDelete(user.userId)}
                          >
                            Sil
                          </button>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Güncelleme Paneli */}
      {editUserId !== null && (
        <div className="position-fixed top-0 start-0 w-100 h-100 bg-dark bg-opacity-50 d-flex justify-content-center align-items-center zindex-tooltip">
          <div className="bg-white rounded shadow p-4 w-100" style={{ maxWidth: '500px' }}>
            <h4 className="mb-4">Kullanıcı Güncelle</h4>
            <div className="mb-3">
              <label className="form-label">Ad</label>
              <input
                type="text"
                value={editFormData.userName}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, userName: e.target.value })
                }
                className="form-control"
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Email</label>
              <input
                type="email"
                value={editFormData.email}
                onChange={(e) =>
                  setEditFormData({ ...editFormData, email: e.target.value })
                }
                className="form-control"
              />
            </div>
            <div className="mb-3">
              <label className="form-label">Rol</label>
              <select
                value={editFormData.role}
                onChange={(e) =>
                  setEditFormData({
                    ...editFormData,
                    role: e.target.value as UpdateUserDto['role'],
                  })
                }
                className="form-select"
              >
                <option value="Admin">Admin</option>
                <option value="Producer">Producer</option>
                <option value="SalesManager">SalesManager</option>
              </select>
            </div>
            <div className="form-check mb-4">
              <input
                type="checkbox"
                className="form-check-input"
                id="isActiveCheck"
                checked={editFormData.isActive}
                onChange={(e) =>
                  setEditFormData({
                    ...editFormData,
                    isActive: e.target.checked,
                  })
                }
              />
              <label className="form-check-label" htmlFor="isActiveCheck">
                Aktif mi?
              </label>
            </div>
            <div className="text-end">
              <button
                className="btn btn-secondary me-2"
                onClick={() => setEditUserId(null)}
              >
                İptal
              </button>
              <button className="btn btn-primary" onClick={handleUpdate}>
                Kaydet
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default UserManagementPage;
