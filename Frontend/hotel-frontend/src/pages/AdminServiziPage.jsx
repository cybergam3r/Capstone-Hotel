import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { toast } from 'react-toastify';

function AdminServiziPage() {
  const [servizi, setServizi] = useState([]);
  const [form, setForm] = useState({ nome: '', prezzo: '' });

  const loadServizi = async () => {
    const res = await api.get('/ServiziExtra');
    setServizi(res.data);
  };

  useEffect(() => {
    loadServizi();
  }, []);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleAdd = async () => {
    await api.post('/ServiziExtra', form);
    setForm({ nome: '', prezzo: '' });
    loadServizi();
    toast.success('Servizio aggiunto!');
  };

  const handleDelete = async (id) => {
    await api.delete(`/ServiziExtra/${id}`);
    loadServizi();
    toast.success('Servizio eliminato!');
  };

  return (
    <div className="container mt-4">
      <h3>Gestione Servizi Extra</h3>

      <div className="mb-4">
        <input name="nome" value={form.nome} onChange={handleChange} placeholder="Nome servizio" className="form-control mb-2" />
        <input name="prezzo" value={form.prezzo} onChange={handleChange} type="number" placeholder="Prezzo" className="form-control mb-2" />
        <button className="btn btn-primary" onClick={handleAdd}>Aggiungi Servizio</button>
      </div>

      <table className="table table-bordered">
        <thead>
          <tr>
            <th>Nome</th>
            <th>Prezzo</th>
            <th>Azioni</th>
          </tr>
        </thead>
        <tbody>
          {servizi.map((s) => (
            <tr key={s.id}>
              <td>{s.nome}</td>
              <td>{s.prezzo}€</td>
              <td>
                <button className="btn btn-sm btn-danger" onClick={() => handleDelete(s.id)}>Elimina</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default AdminServiziPage;