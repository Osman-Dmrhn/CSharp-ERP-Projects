import { BrowserRouter as Router, Routes, Route } from "react-router-dom"; 
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import LoginPage from './pages/LoginPage';
import HomePage from "./pages/HomePage";
import ProtectedRoute from "./routes/ProtectedRoute";
import UserManagementPage from "./pages/UserManagementPage";
import ActivityLogPage from "./pages/ActivityLogPage";
import ProductionOrderPage from "./pages/ProductionOrderPage";
import OrderPage from "./pages/OrderPage";
import StockTransactionPage from "./pages/StockTransactionPage";
import UpdatePasswordPage from "./pages/UpdatePasswordPage";

// Eklenen Importlar
import { AuthProvider } from "./contexts/AuthContext"; 
import AppNavbar from "./components/AppNavbar";     

function App() {
  return (
      <AuthProvider>
        <AppNavbar />
        <Routes>
          <Route path='/login' element={<LoginPage />} />
          <Route 
            path="/" 
            element={
              <ProtectedRoute>
                <HomePage />
              </ProtectedRoute>
            } 
          />
          <Route path="/users" element={
            <ProtectedRoute>
              <UserManagementPage />
            </ProtectedRoute>
          }/>
          <Route path="/logs" element={
            <ProtectedRoute>
              <ActivityLogPage />
            </ProtectedRoute>
          } />
          <Route path="/orders" element={
            <ProtectedRoute>
              <OrderPage />
            </ProtectedRoute>
          } />
          <Route path="/production-orders" element={
            <ProtectedRoute>
              <ProductionOrderPage />
            </ProtectedRoute>
          } />
          <Route path="/stock" element={
            <ProtectedRoute>
              <StockTransactionPage />
            </ProtectedRoute>
          } />
          <Route path="/change-password" element={
            <ProtectedRoute>
              <UpdatePasswordPage />
            </ProtectedRoute>
          }/>
        </Routes>
      </AuthProvider>
  )
}

export default App;