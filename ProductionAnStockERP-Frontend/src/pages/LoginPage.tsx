import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useLogin } from "../hooks/useLogin";
import type { UserLoginRequest } from "../models/LoginDtos/UserLoginRequest";
import "../assets/css/LoginPage.css"

const LoginPage: React.FC = () => {
  const { performLogin, isLoading, error } = useLogin();
  const navigate = useNavigate();
  const location = useLocation();

  const from = location.state?.from?.pathname || "/";

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [formError, setFormError] = useState<string | null>(null);
  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!email.trim() || !password.trim()) {
      setFormError("Lütfen e-posta ve şifreyi girin.");
      return;
    }
    setFormError(null);

    const loginData: UserLoginRequest = { email, passwordHash: password };
    const success = await performLogin(loginData);
    if (success) {
      navigate(from, { replace: true });
    }
  };

  return (
    <div className="login-page">
      <div className="login-box">
        <h2 className="login-title">ERP Sistem Girişi</h2>
        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label htmlFor="email">E-posta</label>
            <input
              id="email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              autoComplete="email"
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Şifre</label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              autoComplete="current-password"
            />
          </div>

          {(formError || error) && (
            <p className="error-message">{formError || error}</p>
          )}

          <button type="submit" disabled={isLoading} className="btn-login">
            {isLoading ? "Giriş Yapılıyor..." : "Giriş Yap"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;