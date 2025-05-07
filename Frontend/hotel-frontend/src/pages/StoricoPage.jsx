import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { Modal, Button, Form } from 'react-bootstrap';
import { toast } from 'react-toastify';

function StoricoPage() {
  const [prenotazioni, setPrenotazioni] = useState([]);
  const [showCancelModal, setShowCancelModal] = useState(false);
  const [prenotazioneSelezionata, setPrenotazioneSelezionata] = useState(null);
  const [motivazione, setMotivazione] = useState('');

  useEffect(() => {
    const fetchPrenotazioni = async () => {
      try {
        const response = await api.get('/prenotazioni');
        setPrenotazioni(response.data);
      } catch (error) {
        console.error('Errore nel caricamento delle prenotazioni:', error);
      }
    };

    fetchPrenotazioni();
  }, []);

  const handleCancellaPrenotazione = async () => {
    if (!motivazione.trim()) {
      toast.error('Devi fornire una motivazione per cancellare la prenotazione.');
      return;
    }

    const oggi = new Date();
    const domani = new Date();
    domani.setDate(oggi.getDate() + 1);

    const checkInDate = new Date(prenotazioneSelezionata.checkIn);

    if (checkInDate.toDateString() === oggi.toDateString() || checkInDate.toDateString() === domani.toDateString()) {
      toast.error('Non puoi cancellare una prenotazione per oggi o domani.');
      setShowCancelModal(false);
      return;
    }

    try {
      await api.delete(`/prenotazioni/${prenotazioneSelezionata.id}`, {
        data: { motivazione },
      });
      toast.success('Prenotazione cancellata con successo.');
      setPrenotazioni((prev) => prev.filter((p) => p.id !== prenotazioneSelezionata.id));
      setShowCancelModal(false);
    } catch (error) {
      console.error('Errore nella cancellazione della prenotazione:', error);
      toast.error('Errore nella cancellazione della prenotazione.');
    }
  };

  return (
    <div className="storico-page">
      <h3 className="text-center my-4">Storico Prenotazioni</h3>

      <div className="prenotazioni-container">
        {prenotazioni.map((prenotazione) => (
          <div key={prenotazione.id} className="camera-card">
            <div className="camera-info">
              <h5>Camera: {prenotazione.camera?.numero || 'Non disponibile'}</h5>
              <p>Check-in: {prenotazione.checkIn ? new Date(prenotazione.checkIn).toLocaleDateString() : 'Non disponibile'}</p>
              <p>Check-out: {prenotazione.checkOut ? new Date(prenotazione.checkOut).toLocaleDateString() : 'Non disponibile'}</p>
              <p>Totale: {prenotazione.totale ? `${prenotazione.totale}€` : 'Non disponibile'}</p>
              <button
                className="btn btn-danger mt-2"
                onClick={() => {
                  setPrenotazioneSelezionata(prenotazione);
                  setShowCancelModal(true);
                }}
              >
                Cancella Prenotazione
              </button>
            </div>
          </div>
        ))}
      </div>

     
      <Modal show={showCancelModal} onHide={() => setShowCancelModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Cancella Prenotazione</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Sei sicuro di voler cancellare questa prenotazione?</p>
          <Form.Group>
            <Form.Label>Motivazione</Form.Label>
            <Form.Control
              as="textarea"
              rows={3}
              value={motivazione}
              onChange={(e) => setMotivazione(e.target.value)}
              placeholder="Scrivi la motivazione per la cancellazione..."
            />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowCancelModal(false)}>
            Annulla
          </Button>
          <Button variant="danger" onClick={handleCancellaPrenotazione}>
            Conferma Cancellazione
          </Button>
        </Modal.Footer>
      </Modal>
    </div>
  );
}

export default StoricoPage;