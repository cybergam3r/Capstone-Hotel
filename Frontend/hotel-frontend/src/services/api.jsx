import axios from "axios";
import { store } from "../redux/store"; 

const api = axios.create({
  baseURL: "https://localhost:7213/api",
});


api.interceptors.request.use((config) => {
  const state = store.getState();
  const token = state.auth.token;

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export default api;