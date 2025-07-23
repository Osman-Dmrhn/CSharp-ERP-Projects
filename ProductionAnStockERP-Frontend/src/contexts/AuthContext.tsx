// Dosya: src/contexts/AuthProvider.tsx

import { createContext, useContext, useEffect, useState } from 'react';
import type { ReactNode } from 'react';
import authService from '../api/authService';
import { getMe } from '../api/UserApi';
import type { UserLoginRequest } from '../models/LoginDtos/UserLoginRequest';
import type { User } from '../models/UserDtos/User';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  login: (credentials: UserLoginRequest) => Promise<void>;
  logout: () => void;
  loading: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  // LOG 1: Provider'ın her render olduğunda o anki state'ini görelim
  console.log('%cAuthProvider Render Edildi:', 'color: orange; font-weight: bold;', { loading, user });

  const checkSession = async () => {
    // LOG 2
    console.log('%c[1] checkSession Başladı', 'color: blue');
    setLoading(true);
    const token = localStorage.getItem('token');
    if (!token) {
      // LOG 3
      console.log('%c[2] Token bulunamadı.', 'color: red');
      setUser(null);
      setLoading(false);
      return;
    }
    try {
      // LOG 4
      console.log('%c[2] Token bulundu, kullanıcı bilgisi isteniyor...', 'color: green');
      const userData = await getMe();
      // LOG 5
      console.log('%c[3] Kullanıcı bilgisi geldi:', 'color: green; font-weight: bold;', userData);
      setUser(userData);
    } catch (error) {
      console.error('Oturum doğrulama hatası:', error);
      setUser(null);
      localStorage.removeItem('token');
    } finally {
      // LOG 6
      console.log('%c[4] checkSession Bitti, loading false yapılıyor.', 'color: blue');
      setLoading(false);
    }
  };

  const login = async (credentials: UserLoginRequest) => {
    // LOG 7
    console.log('%c[A] Login fonksiyonu çağrıldı.', 'color: purple; font-weight: bold;');
    try {
      const response = await authService.login(credentials);
      if (response.success && response.data) {
        localStorage.setItem('token', response.data);
        // LOG 8
        console.log('%c[B] Token kaydedildi, checkSession tetikleniyor.', 'color: purple');
        await checkSession();
        // LOG 9
        console.log('%c[C] checkSession bitti, login fonksiyonu tamamlandı.', 'color: purple; font-weight: bold;');
      } else {
        throw new Error(response.message || 'Giriş başarısız.');
      }
    } catch (error) {
      console.error('Giriş hatası:', error);
      throw error;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
    window.location.href = '/login';
  };

  useEffect(() => {
    // Sayfa ilk yüklendiğinde oturumu kontrol et
    checkSession();
  }, []);

  return (
    <AuthContext.Provider value={{ user, isAuthenticated: !!user, login, logout, loading }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth hook must be used within an AuthProvider');
  }
  return context;
};