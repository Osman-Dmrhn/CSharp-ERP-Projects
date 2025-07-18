import { Routes, Route } from "react-router-dom";
import './App.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';
import LoginPage from './pages/LoginPage';
import HomePage from "./pages/HomePage";
import ProtectedRoute from "./routes/ProtectedRoute";

function App() {
  return (
   <Routes>
      
      <Route path='/login' element={<LoginPage/>}></Route>

      <Route 
          path="/" 
          element={
            <ProtectedRoute>
              <HomePage/>
            </ProtectedRoute>
          } 
        />
   </Routes>
  )
}

export default App
