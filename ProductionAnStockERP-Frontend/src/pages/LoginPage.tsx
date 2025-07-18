import React, { useState } from "react";
// 1. Gerekli hook'ları react-router-dom'dan import ediyoruz.
import { useNavigate, useLocation } from "react-router-dom";
import { useLogin } from "../hooks/useLogin";
import type { UserLoginRequest } from "../models/LoginDtos/UserLoginRequest";

const LoginPage: React.FC = () => {
  const { performLogin, isLoading, error } = useLogin();

  const navigate = useNavigate();
  const location = useLocation();

  const from = location.state?.from?.pathname || "/";

  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [formError, setFormError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!email || !password) {
      setFormError("Lütfen e-posta ve şifreyi doldurun.");
      return;
    }

    setFormError(null);

    const loginData: UserLoginRequest = {
      email,
      passwordHash: password,
    };

    const success = await performLogin(loginData);

    if (success) {

      console.log(`Yönlendirme deneniyor... Hedef: ${from}`); 
      navigate(from, { replace: true });
    }
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md bg-white p-8 rounded shadow-lg">
        <h2 className="text-2xl font-bold mb-6 text-center">Giriş Yap</h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
              E-posta
            </label>
            <input
              type="email"
              id="email"
              className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              autoComplete="email"
            />
          </div>

          <div>
            <label htmlFor="password" className="block text-sm font-medium text-gray-700">
              Şifre
            </label>
            <input
              type="password"
              id="password"
              className="mt-1 block w-full border border-gray-300 rounded-md px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              autoComplete="current-password"
            />
          </div>

          {formError && <p className="text-red-600 text-sm text-center">{formError}</p>}
          {error && <p className="text-red-600 text-sm text-center">{error}</p>}

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-blue-600 text-white py-2 rounded-md hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:bg-blue-300 transition-colors"
          >
            {isLoading ? "Giriş Yapılıyor..." : "Giriş Yap"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage