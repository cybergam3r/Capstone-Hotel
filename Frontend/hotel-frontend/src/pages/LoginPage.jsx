import React, { useState } from "react";
import { useDispatch } from "react-redux";
import axios from "axios";
import { loginSuccess } from "../redux/slices/authSlice";
import { useNavigate } from "react-router-dom";

function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post("https://localhost:7213/api/auth/login", {
        email,
        password,
      });

      const { token, email: userEmail, role } = response.data;

      
      dispatch(loginSuccess({ token, user: { email: userEmail, role } }));

      navigate("/dashboard");
    } catch (err) {
      alert("Login fallito. Controlla le credenziali.");
      console.error(err);
    }
  };

  return (
    <div className="container mt-5" style={{ maxWidth: "400px" }}>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div className="mb-3">
          <label>Email</label>
          <input
            type="email"
            className="form-control"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label>Password</label>
          <input
            type="password"
            className="form-control"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary w-100">
          Accedi
        </button>
        <p className="mt-3 text-center">
          Non hai un account? <a href="/register">Registrati</a>
        </p>
      </form>
    </div>
  );
}

export default LoginPage;