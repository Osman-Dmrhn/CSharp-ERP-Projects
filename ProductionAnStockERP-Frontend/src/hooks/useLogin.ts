import { useState } from "react";
import authService from "../api/authService";
import type { UserLoginRequest } from "../models/LoginDtos/UserLoginRequest";

export const useLogin = () => {
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const performLogin = async (loginData: UserLoginRequest):Promise<boolean> =>  {
    setIsLoading(true);
    setError(null);

    try {
      const loginResponse = await authService.login(loginData);
      const token = loginResponse.data;

      localStorage.setItem("token", token);
      console.log("Giriş başarılı!");
      setIsLoading(false);
      return true;
    } catch (err: any) {

      setError(err?.message || "Login işlemi başarısız oldu.");
      localStorage.removeItem("token");
      setIsLoading(false);
      return false;

    } finally {
      
      setIsLoading(false);
    }
  };
  return { performLogin, isLoading, error };
};
