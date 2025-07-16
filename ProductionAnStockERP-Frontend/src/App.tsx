import { useState } from 'react'
import { Routes, Route,Navigate } from "react-router-dom";
import './App.css'
import LoginPage from './pages/LoginPage';

function App() {
  const [count, setCount] = useState(0)

  return (
   <Routes>
      <Route path="/" element={<Navigate to="/login" replace />} />
      <Route path='/login' element={<LoginPage/>}></Route>
   </Routes>
  )
}

export default App
