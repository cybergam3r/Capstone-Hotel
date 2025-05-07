import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { useSelector } from "react-redux";

import AppNavbar from "./components/Navbar";
import PrivateRoute from "./components/PrivateRoute";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import DashboardPage from "./pages/DashboardPage";
import CamerePage from "./pages/CamerePage";
import StoricoPage from "./pages/StoricoPage";
import AdminCamerePage from "./pages/AdminCamerePage";
import AdminPrenotazioniPage from "./pages/AdminPrenotazioniPage";
import AdminUtentiPage from "./pages/AdminUtentiPage";
import AdminServiziPage from "./pages/AdminServiziPage"; 
import ProfiloPage from './pages/ProfiloPage';
import HomePage from "./pages/HomePage";
import './App.css';


function AdminRoute({ children }) {
  const user = useSelector((state) => state.auth.user);
  if (!user) return <Navigate to="/login" />; 
  return user.role === "Admin" ? children : <Navigate to="/dashboard" />;
}

function App() {
  return (
    <Router>
      <AppNavbar />
      <Routes>
        
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<HomePage />} />

        
        <Route
          path="/dashboard"
          element={
            <PrivateRoute>
              <DashboardPage />
            </PrivateRoute>
          }
        />
        <Route
          path="/camere"
          element={
            <PrivateRoute>
              <CamerePage />
            </PrivateRoute>
          }
        />
        <Route
          path="/storico"
          element={
            <PrivateRoute>
              <StoricoPage />
            </PrivateRoute>
          }
        />

<Route path="/profilo" element={<ProfiloPage />} />

        
        <Route
          path="/admin/camere"
          element={
            <PrivateRoute>
              <AdminRoute>
                <AdminCamerePage />
              </AdminRoute>
            </PrivateRoute>
          }
        />
        <Route
          path="/admin/prenotazioni"
          element={
            <PrivateRoute>
              <AdminRoute>
                <AdminPrenotazioniPage />
              </AdminRoute>
            </PrivateRoute>
          }
        />
        <Route
          path="/admin/utenti"
          element={
            <PrivateRoute>
              <AdminRoute>
                <AdminUtentiPage />
              </AdminRoute>
            </PrivateRoute>
          }
        />
        <Route
          path="/admin/servizi"
          element={
            <PrivateRoute>
              <AdminRoute>
                <AdminServiziPage />
              </AdminRoute>
            </PrivateRoute>
          }
        />

       
        <Route path="*" element={<Navigate to="/dashboard" />} />
      </Routes>
    </Router>
  );
}

export default App;

