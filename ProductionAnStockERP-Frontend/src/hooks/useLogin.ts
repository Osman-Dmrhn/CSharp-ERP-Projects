import { useState } from "react";
import authService from "../api/authService";
import type { UserLoginRequest } from "../models/UserLoginRequest";

export const useLogin = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const performLogin = async (loginData: UserLoginRequest) => {
    setIsLoading(true);
    setError(null);

    try {
      const loginResponse = await authService.login(loginData);
      const token = loginResponse.data;

      localStorage.setItem("token", token);
    } catch (err: any) {
      setError(err?.message || "Login işlemi başarısız oldu.");
      localStorage.removeItem("token");
    } finally {
      setIsLoading(false);
    }
  };

  return { performLogin, isLoading, error };
};
