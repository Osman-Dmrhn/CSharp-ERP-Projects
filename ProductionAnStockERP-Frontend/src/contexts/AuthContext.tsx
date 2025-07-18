import React, { createContext, useState, useEffect, useContext} from 'react';
import type { ReactNode } from 'react';
import { getMe } from '../api/UserApi';
import type { User } from '../models/UserDtos/User';

// Context'in içinde tutulacak verilerin ve fonksiyonların tipini belirliyoruz
interface AuthContextType {
  isAuthenticated: boolean;
  user: User | null;
  isLoading: boolean;
  logout: () => void;
}

// Context'i oluşturuyoruz
const AuthContext = createContext<AuthContextType | undefined>(undefined);

// Diğer bileşenleri sarmalayacak olan Provider bileşenimiz
export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true); // Başlangıçta yükleniyor durumunda

  useEffect(() => {
    const validateTokenAndFetchUser = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setIsLoading(false);
        return;
      }

      try {
        // Token varsa, /me endpoint'i ile kullanıcıyı doğrula ve bilgilerini al
        const userData = await getMe();
        setUser(userData);
      } catch (error) {
        // Eğer token geçersizse veya bir hata oluşursa, kullanıcı bilgilerini temizle
        console.error("Token doğrulama hatası:", error);
        setUser(null);
        localStorage.removeItem('token');
      } finally {
        // Her durumda yüklenme durumunu bitir
        setIsLoading(false);
      }
    };

    validateTokenAndFetchUser();
  }, []); // Bu useEffect sadece uygulama ilk yüklendiğinde 1 kez çalışır

  const logout = () => {
    setUser(null);
    localStorage.removeItem('token');
    // Login sayfasına yönlendirme de burada yapılabilir
    window.location.href = '/login';
  };

  // Eğer başlangıçta hala kullanıcı verisi çekiliyorsa, bir yüklenme ekranı göster
  if (isLoading) {
   
  }

  // Context'in değerlerini Provider aracılığıyla alt bileşenlere sunuyoruz
  return (
    <AuthContext.Provider value={{ isAuthenticated: !!user, user, isLoading, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

// Context'i diğer bileşenlerde kolayca kullanmak için bir custom hook
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth, AuthProvider içinde kullanılmalıdır');
  }
  return context;
};