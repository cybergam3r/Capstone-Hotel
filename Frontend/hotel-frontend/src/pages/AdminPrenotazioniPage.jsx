import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { toast } from 'react-toastify';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { Modal, Button, Form } from 'react-bootstrap';

function AdminPrenotazioniPage() {
  const [prenotazioni, setPrenotazioni] = useState([]);
  const [loading, setLoading] = useState(true);

  const [filtroEmail, setFiltroEmail] = useState('');
  const [filtroDataInizio, setFiltroDataInizio] = useState(null);
  const [filtroDataFine, setFiltroDataFine] = useState(null);

  const [prenotazioneSelezionata, setPrenotazioneSelezionata] = useState(null);
  const [camereDisponibili, setCamereDisponibili] = useState([]);
  const [serviziDisponibili, setServiziDisponibili] = useState([]);
  const [showModificaModal, setShowModificaModal] = useState(false);

  useEffect(() => {
    const fetchAllPrenotazioni = async () => {
      try {
        const res = await api.get('/prenotazioni/all');
        setPrenotazioni(res.data);
      } catch (err) {
        toast.error('Errore nel caricamento delle prenotazioni.');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchAllPrenotazioni();
  }, []);

  const filtraPrenotazioni = prenotazioni.filter((p) => {
    const emailMatch = p.userEmail?.toLowerCase().includes(filtroEmail.toLowerCase());
    const dataInizio = new Date(p.checkIn);
    const inizioValido = filtroDataInizio ? dataInizio >= filtroDataInizio : true;
    const fineValido = filtroDataFine ? dataInizio <= filtroDataFine : true;
    return emailMatch && inizioValido && fineValido;
  });

  const handleEliminaPrenotazione = async (id) => {
    try {
      await api.delete(`/prenotazioni/${id}`);
      setPrenotazioni((prev) => prev.filter((p) => p.id !== id));
      toast.success('Prenotazione eliminata con successo.');
    } catch (err) {
      toast.error('Errore durante l\'eliminazione della prenotazione.');
      console.error(err);
    }
  };

  const handleModificaPrenotazione = async (prenotazione) => {
    setPrenotazioneSelezionata(prenotazione);

    try {
      
      const camereResponse = await api.get('/Camere/disponibili-per-intervallo', {
        params: {
          dataInizio: prenotazione.checkIn,
          dataFine: prenotazione.checkOut,
        },
      });

      let camereDisponibiliAggiornate = camereResponse.data;

      
      let cameraSelezionata = null;
      if (prenotazione.cameraId) {
        const cameraResponse = await api.get(`/Camere/${prenotazione.cameraId}`);
        cameraSelezionata = cameraResponse.data;
      }

      
      if (cameraSelezionata && !camereDisponibiliAggiornate.some((c) => c.id === cameraSelezionata.id)) {
        camereDisponibiliAggiornate = [...camereDisponibiliAggiornate, cameraSelezionata];
      }

      
      camereDisponibiliAggiornate.sort((a, b) => a.id - b.id);

      setCamereDisponibili(camereDisponibiliAggiornate);

      
      const serviziResponse = await api.get('/serviziExtra');
      setServiziDisponibili(serviziResponse.data);
    } catch (err) {
      toast.error('Errore nel caricamento delle camere o dei servizi disponibili.');
      console.error(err);
    }

    setShowModificaModal(true);
  };

  const toggleServizio = (servizio) => {
    const serviziSelezionati = prenotazioneSelezionata.servizi.some((s) => s.id === servizio.id)
      ? prenotazioneSelezionata.servizi.filter((s) => s.id !== servizio.id)
      : [...prenotazioneSelezionata.servizi, servizio];

    setPrenotazioneSelezionata((prev) => ({ ...prev, servizi: serviziSelezionati }));
  };

  const salvaModifichePrenotazione = async () => {
    try {
      const { id, checkIn, checkOut, totale, servizi, cameraId } = prenotazioneSelezionata;
      await api.put(`/prenotazioni/${id}`, {
        checkIn,
        checkOut,
        totale,
        servizi,
        cameraId,
      });
      setPrenotazioni((prev) =>
        prev.map((p) => (p.id === id ? prenotazioneSelezionata : p))
      );
      toast.success('Prenotazione modificata con successo.');
      setShowModificaModal(false);
    } catch (err) {
      toast.error('Errore durante la modifica della prenotazione.');
      console.error(err);
    }
  };

  if (loading) return <div className="container mt-4">Caricamento prenotazioni...</div>;

  return (
    <div className="container mt-4">
      <h3 className="mb-3">Gestione Prenotazioni</h3>

     
      <div className="row mb-4">
        <div className="col-md-3">
          <label className="form-label">Filtra per email</label>
          <input
            type="text"
            className="form-control"
            placeholder="utente@email.com"
            value={filtroEmail}
            onChange={(e) => setFiltroEmail(e.target.value)}
          />
        </div>
        <div className="col-md-3">
          <label className="form-label">Data inizio da</label>
          <DatePicker
            selected={filtroDataInizio}
            onChange={(date) => setFiltroDataInizio(date)}
            className="form-control"
            dateFormat="dd/MM/yyyy"
            placeholderText="Inizio"
            isClearable
          />
        </div>
        <div className="col-md-3">
          <label className="form-label">a</label>
          <DatePicker
            selected={filtroDataFine}
            onChange={(date) => setFiltroDataFine(date)}
            className="form-control"
            dateFormat="dd/MM/yyyy"
            placeholderText="Fine"
            isClearable
          />
        </div>
        <div className="col-md-3 d-flex align-items-end">
          <button
            className="btn btn-secondary w-100"
            onClick={() => {
              setFiltroEmail('');
              setFiltroDataInizio(null);
              setFiltroDataFine(null);
            }}
          >
            Reset filtri
          </button>
        </div>
      </div>

     
      {filtraPrenotazioni.length === 0 ? (
        <div className="alert alert-info">Nessuna prenotazione trovata.</div>
      ) : (
        <table className="table table-striped table-bordered">
          <thead className="table-dark">
            <tr>
              <th>Email Utente</th>
              <th>Numero Camera</th>
              <th>Check-In</th>
              <th>Check-Out</th>
              <th>Totale</th>
              <th>Servizi</th>
              <th>Azioni</th>
            </tr>
          </thead>
          <tbody>
            {filtraPrenotazioni.map((p) => (
              <tr key={p.id}>
                <td>{p.userEmail}</td>
                <td>{p.camera?.numero}</td>
                <td>{new Date(p.checkIn).toLocaleDateString()}</td>
                <td>{new Date(p.checkOut).toLocaleDateString()}</td>
                <td>{p.totale.toFixed(2)} €</td>
                <td>
                  {p.servizi?.length > 0 ? (
                    <ul>
                      {p.servizi.map((servizio) => (
                        <li key={servizio.id}>
                          {servizio.nome} (+{servizio.prezzo}€)
                        </li>
                      ))}
                    </ul>
                  ) : (
                    'Nessun servizio aggiuntivo'
                  )}
                </td>
                <td>
                  <button
                    className="btn btn-sm btn-warning me-2"
                    onClick={() => handleModificaPrenotazione(p)}
                  >
                    Modifica
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleEliminaPrenotazione(p.id)}
                  >
                    Elimina
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}

     
      {prenotazioneSelezionata && (
        <Modal show={showModificaModal} onHide={() => setShowModificaModal(false)}>
          <Modal.Header closeButton>
            <Modal.Title>Modifica Prenotazione</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form>
              <Form.Group className="mb-3">
                <Form.Label>Check-In</Form.Label>
                <DatePicker
                  selected={new Date(prenotazioneSelezionata.checkIn)}
                  onChange={(date) => {
                    setPrenotazioneSelezionata((prev) => ({ ...prev, checkIn: date }));
                  }}
                  className="form-control"
                  dateFormat="dd/MM/yyyy"
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Check-Out</Form.Label>
                <DatePicker
                  selected={new Date(prenotazioneSelezionata.checkOut)}
                  onChange={(date) => {
                    setPrenotazioneSelezionata((prev) => ({ ...prev, checkOut: date }));
                  }}
                  className="form-control"
                  dateFormat="dd/MM/yyyy"
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Totale</Form.Label>
                <Form.Control
                  type="number"
                  value={prenotazioneSelezionata.totale}
                  onChange={(e) =>
                    setPrenotazioneSelezionata((prev) => ({
                      ...prev,
                      totale: parseFloat(e.target.value),
                    }))
                  }
                />
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Camera</Form.Label>
                <Form.Select
                  value={prenotazioneSelezionata.cameraId}
                  onChange={(e) => {
                    const nuovaCameraId = parseInt(e.target.value);
                    setPrenotazioneSelezionata((prev) => ({
                      ...prev,
                      cameraId: nuovaCameraId,
                    }));
                  }}
                >
                  <option value="">Seleziona una camera</option>
                  {camereDisponibili.map((camera) => (
                    <option key={camera.id} value={camera.id}>
                      {camera.numero} - {camera.tipo} ({camera.prezzoNotte}€/notte)
                    </option>
                  ))}
                </Form.Select>
              </Form.Group>
              <Form.Group className="mb-3">
                <Form.Label>Servizi</Form.Label>
                {serviziDisponibili.map((servizio) => (
                  <Form.Check
                    key={servizio.id}
                    type="checkbox"
                    label={`${servizio.nome} (+${servizio.prezzo}€)`}
                    onChange={() => toggleServizio(servizio)}
                    checked={prenotazioneSelezionata.servizi.some((s) => s.id === servizio.id)}
                  />
                ))}
              </Form.Group>
            </Form>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => setShowModificaModal(false)}>
              Annulla
            </Button>
            <Button variant="primary" onClick={salvaModifichePrenotazione}>
              Salva Modifiche
            </Button>
          </Modal.Footer>
        </Modal>
      )}
    </div>
  );
}

export default AdminPrenotazioniPage;