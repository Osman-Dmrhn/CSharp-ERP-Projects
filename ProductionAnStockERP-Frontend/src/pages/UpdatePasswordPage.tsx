import React, { useState } from 'react';
import {updateUserPassword} from '../api/UserService'; 
import type { UpdateUserPasswordDto } from '../models/UserDtos/UpdateUserPasswordDto';

const UpdatePasswordPage: React.FC = () => {
  const [formData, setFormData] = useState<UpdateUserPasswordDto>({
    userId: 1,  
    oldpass: '',
    newpass: '',
  });

  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState<string>('');
  const [successMessage, setSuccessMessage] = useState<string>('');

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setErrorMessage('');
    setSuccessMessage('');

    try {
      const result = await updateUserPassword(formData);
      if (result.success) {
        setSuccessMessage('Şifreniz başarıyla güncellendi!');
        setFormData({
          userId: formData.userId,
          oldpass: '',
          newpass: '',
        });
      } else {
        setErrorMessage('Şifre güncellenirken bir hata oluştu.');
      }
    } catch (error) {
      setErrorMessage('Bir hata oluştu, lütfen tekrar deneyin.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container-fluid min-h-screen bg-light py-5 px-3">
      <div className="row justify-content-center">
        <div className="col-lg-6 col-md-8 col-12">
          <div className="bg-white rounded shadow p-4">
            <h2 className="text-primary mb-4">Şifre Güncelleme</h2>
            

            {errorMessage && <div className="alert alert-danger">{errorMessage}</div>}
            {successMessage && <div className="alert alert-success">{successMessage}</div>}
            
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label className="form-label">Eski Şifre</label>
                <input
                  type="password"
                  className="form-control"
                  name="oldpass"
                  value={formData.oldpass}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Yeni Şifre</label>
                <input
                  type="password"
                  className="form-control"
                  name="newpass"
                  value={formData.newpass}
                  onChange={handleInputChange}
                  required
                />
              </div>
              <div className="text-end">
                <button type="submit" className="btn btn-primary" disabled={loading}>
                  {loading ? 'Yükleniyor...' : 'Şifreyi Güncelle'}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UpdatePasswordPage;
