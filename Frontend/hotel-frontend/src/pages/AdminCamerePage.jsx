import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { Modal, Button } from 'react-bootstrap';

function AdminCamerePage() {
  const [camere, setCamere] = useState([]);
  const [form, setForm] = useState({ numero: '', tipo: '', prezzoNotte: '', disponibile: true });
  const [servizi, setServizi] = useState([]);

  const [cameraSelezionata, setCameraSelezionata] = useState(null);
  const [showModal, setShowModal] = useState(false);

  const loadCamere = async () => {
    const res = await api.get('/camere');
    setCamere(res.data);
  };

  const loadServizi = async () => {
    const res = await api.get('/serviziextra');
    setServizi(res.data);
  };

  useEffect(() => {
    loadCamere();
    loadServizi();
  }, []);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm({ ...form, [name]: type === 'checkbox' ? checked : value });
  };

  const handleAdd = async () => {
    await api.post('/camere', form);
    setForm({ numero: '', tipo: '', prezzoNotte: '', disponibile: true });
    loadCamere();
  };

  const handleDelete = async (id) => {
    await api.delete(`/camere/${id}`);
    loadCamere();
  };

  const openDettagli = (camera) => {
    setCameraSelezionata(camera);
    setShowModal(true);
  };

  return (
    <div className="container mt-4">
      <h3>Gestione Camere</h3>

      <div className="mb-4">
        <input name="numero" value={form.numero} onChange={handleChange} placeholder="Numero" className="form-control mb-2" />
        <input name="tipo" value={form.tipo} onChange={handleChange} placeholder="Tipo" className="form-control mb-2" />
        <input name="prezzoNotte" value={form.prezzoNotte} onChange={handleChange} type="number" placeholder="Prezzo" className="form-control mb-2" />
        <div className="form-check mb-2">
          <input
            className="form-check-input"
            type="checkbox"
            name="disponibile"
            checked={form.disponibile}
            onChange={handleChange}
            id="disponibileCheck"
          />
          <label className="form-check-label" htmlFor="disponibileCheck">
            Disponibile
          </label>
        </div>
        <button className="btn btn-primary" onClick={handleAdd}>Aggiungi Camera</button>
      </div>

      <table className="table table-bordered">
        <thead>
          <tr>
            <th>Numero</th>
            <th>Tipo</th>
            <th>Prezzo</th>
            <th>Disponibile</th>
            <th>Azioni</th>
          </tr>
        </thead>
        <tbody>
          {camere.map((c) => (
            <tr key={c.id}>
              <td>{c.numero}</td>
              <td>{c.tipo}</td>
              <td>{c.prezzoNotte}€</td>
              <td>{c.disponibile ? '✅' : '❌'}</td>
              <td>
                <button className="btn btn-sm btn-info me-2" onClick={() => openDettagli(c)}>Dettagli</button>
                <button className="btn btn-sm btn-danger" onClick={() => handleDelete(c.id)}>Elimina</button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      <Modal show={showModal} onHide={() => setShowModal(false)} size="lg">
        <Modal.Header closeButton>
          <Modal.Title>Dettagli Camera {cameraSelezionata?.numero}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p><strong>Tipo:</strong> {cameraSelezionata?.tipo}</p>
          <p><strong>Prezzo/notte:</strong> {cameraSelezionata?.prezzoNotte}€</p>
          <p><strong>Disponibile:</strong> {cameraSelezionata?.disponibile ? 'Sì' : 'No'}</p>

          <hr />
          <h5>Servizi Disponibili</h5>
          <ul>
            {servizi.map((s) => (
              <li key={s.id}>
                {s.nome} - {s.prezzo}€
              </li>
            ))}
          </ul>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowModal(false)}>
            Chiudi
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}

export default AdminCamerePage;
