import React, { useState } from 'react';
import api from '../services/api';
import { toast } from 'react-toastify';
import { useNavigate } from 'react-router-dom';

function RegisterPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      await api.post('/auth/register', {
        email,
        password
        
      });
      toast.success('Registrazione completata!');
      navigate('/login');
    } catch (error) {
      if (error.response?.data?.errors) {
        
        const errs = error.response.data.errors;
        const messages = Object.values(errs).flat();
        messages.forEach((msg) => toast.error(msg));
      } else {
        toast.error('Errore nella registrazione.');
      }
      console.error(error);
    }
  };

  return (
    <div className="container mt-4" style={{ maxWidth: '400px' }}>
      <h3>Registrazione</h3>
      <form onSubmit={handleRegister}>
        <div className="mb-3">
          <label className="form-label">Email</label>
          <input
            type="email"
            className="form-control"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="nome@email.com"
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Password</label>
          <input
            type="password"
            className="form-control"
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Password sicura"
          />
        </div>
        <button type="submit" className="btn btn-primary w-100">
          Registrati
        </button>
      </form>
    </div>
  );
}

export default RegisterPage;