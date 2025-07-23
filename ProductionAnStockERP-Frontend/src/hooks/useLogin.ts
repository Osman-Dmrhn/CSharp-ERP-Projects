// Dosya: src/hooks/useLogin.ts

import { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import type { UserLoginRequest } from '../models/LoginDtos/UserLoginRequest';

export const useLogin = () => {
  // Bu state'ler SADECE login formundaki butona ve hata mesajına özeldir.
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Merkezi AuthProvider'daki login fonksiyonunu alıyoruz.
  const { login } = useAuth();

  const performLogin = async (credentials: UserLoginRequest): Promise<boolean> => {
    setIsLoading(true);
    setError(null);

    try {
      await login(credentials);

      setIsLoading(false);
      return true; 
    } catch (err: any) {
      setError(err.message || 'Giriş sırasında bir hata oluştu.');
      setIsLoading(false);
      return false;
    }
  };

  return { performLogin, isLoading, error };
};