import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { Modal, Button, Form, Carousel } from 'react-bootstrap';
import DatePicker from 'react-datepicker';
import 'react-datepicker/dist/react-datepicker.css';
import { toast } from 'react-toastify';

function CamerePage() {
  const [camere, setCamere] = useState([]);
  const [loading, setLoading] = useState(true);

  const [dataInizio, setDataInizio] = useState(new Date());
  const [dataFine, setDataFine] = useState(new Date(new Date().setDate(new Date().getDate() + 1)));

  const [cameraSelezionata, setCameraSelezionata] = useState(null);
  const [showServiziModal, setShowServiziModal] = useState(false);
  const [showConfermaModal, setShowConfermaModal] = useState(false);
  const [showProfiloModal, setShowProfiloModal] = useState(false);
  const [expandedCamera, setExpandedCamera] = useState(null); 

  const [servizi, setServizi] = useState([]);
  const [serviziSelezionati, setServiziSelezionati] = useState([]);
  const [totale, setTotale] = useState(0);

  useEffect(() => {
    const fetchCamere = async () => {
      try {
        toast.dismiss();
        const response = await api.get('/camere');
        setCamere(response.data);
      } catch (error) {
        toast.dismiss();
        toast.error('Errore nel caricamento delle camere.');
        console.error(error);
      }
    };

    const fetchServizi = async () => {
      try {
        toast.dismiss();
        const response = await api.get('/serviziExtra');
        setServizi(response.data);
      } catch (error) {
        toast.dismiss();
        toast.error('Errore nel caricamento dei servizi.');
        console.error(error);
      }
    };

    const aggiornaCamerePerOggi = async () => {
      try {
        toast.dismiss();
        await cercaCamerePerDate();
      } catch (error) {
        toast.dismiss();
        console.error('Errore durante l\'aggiornamento delle camere per oggi:', error);
      }
    };

    const fetchData = async () => {
      setLoading(true);
      await fetchCamere();
      await fetchServizi();
      await aggiornaCamerePerOggi();
      setLoading(false);
    };

    fetchData();
  }, []);

  const cercaCamerePerDate = async () => {
    try {
      toast.dismiss();
      const response = await api.get('/Camere/disponibili-per-intervallo', {
        params: {
          dataInizio: dataInizio.toISOString().split('T')[0],
          dataFine: dataFine.toISOString().split('T')[0],
        },
      });
      setCamere(response.data);
    } catch (error) {
      toast.dismiss();
      if (error.response) {
        console.error('Errore nella risposta:', error.response);
        toast.error(`Errore ${error.response.status}: ${error.response.data}`);
      } else if (error.request) {
        console.error('Errore nella richiesta:', error.request);
        toast.error('Errore nella comunicazione con il server.');
      } else {
        console.error('Errore generico:', error.message);
        toast.error('Errore sconosciuto.');
      }
    }
  };

  const handlePrenota = async (cameraId) => {
    setCameraSelezionata(cameraId);
    setShowServiziModal(true);
  };

  const calcolaTotale = () => {
    const camera = camere.find((c) => c.id === cameraSelezionata);
    const prezzoServizi = serviziSelezionati.reduce((acc, servizio) => acc + servizio.prezzo, 0);
    const totalePrenotazione = camera.prezzoNotte + prezzoServizi;
    setTotale(totalePrenotazione);
  };

  const confermaPrenotazione = async () => {
    try {
      const profiloResponse = await api.get('/account/profilo');
      const { nome, cognome, codiceFiscale, dataDiNascita, numeroDiTelefono } = profiloResponse.data;

      if (!nome || !cognome || !codiceFiscale || !dataDiNascita || !numeroDiTelefono) {
        setShowConfermaModal(false);
        setShowProfiloModal(true);
        return;
      }

      await api.post('/prenotazioni', {
        cameraId: cameraSelezionata,
        checkIn: dataInizio.toISOString(),
        checkOut: dataFine.toISOString(),
        servizi: serviziSelezionati,
        totale,
      });

      toast.success('Prenotazione effettuata con successo!');
      setShowConfermaModal(false);
    } catch (error) {
      toast.dismiss();
      toast.error('Errore nella prenotazione.');
      console.error(error);
    }
  };

  const toggleServizio = (servizio) => {
    setServiziSelezionati((prev) =>
      prev.includes(servizio) ? prev.filter((s) => s !== servizio) : [...prev, servizio]
    );
  };

  if (loading) {
    return <div className="container mt-4">Caricamento in corso...</div>;
  }

  return (
    <div className="camere-page">
      <h3 className="text-center my-4">Camere Disponibili</h3>

     
      <div className="d-flex gap-3 align-items-center mb-4 flex-wrap justify-content-center">
        <div>
          <label className="form-label">Dal</label>
          <DatePicker
            selected={dataInizio}
            onChange={(date) => setDataInizio(date)}
            className="form-control"
            dateFormat="dd/MM/yyyy"
          />
        </div>
        <div>
          <label className="form-label">Al</label>
          <DatePicker
            selected={dataFine}
            onChange={(date) => setDataFine(date)}
            className="form-control"
            dateFormat="dd/MM/yyyy"
          />
        </div>
        <div>
          <button className="btn btn-primary mt-4" onClick={cercaCamerePerDate}>
            Cerca Camere
          </button>
        </div>
      </div>

     
      <div className="camere-container">
  {camere.map((camera) => (
    <div
      key={camera.id}
      className={`camera-card ${expandedCamera === camera.id ? 'expanded' : ''}`}
      onClick={() => setExpandedCamera(expandedCamera === camera.id ? null : camera.id)}
    >
      <Carousel>
        <Carousel.Item>
          <img
            className="d-block w-100"
            src={`/images/${camera.tipo.toLowerCase()}1.jpg`}
            alt={`Camera ${camera.tipo}`}
          />
        </Carousel.Item>
        <Carousel.Item>
          <img
            className="d-block w-100"
            src={`/images/${camera.tipo.toLowerCase()}2.jpg`}
            alt={`Camera ${camera.tipo}`}
          />
        </Carousel.Item>
      </Carousel>
      <div className="camera-info">
        <h5>{camera.tipo}</h5>
        <p>Prezzo: {camera.prezzoNotte}€ / notte</p>
      </div>
      {expandedCamera === camera.id && (
        <div className="camera-details">
          <p>
            Questa camera {camera.tipo.toLowerCase()} offre un'esperienza unica con comfort e stile.
            Perfetta per un soggiorno indimenticabile.
          </p>
          {camera.disponibile && (
            <button
              className="btn btn-success"
              onClick={(e) => {
                e.stopPropagation(); 
                handlePrenota(camera.id);
              }}
            >
              Prenota
            </button>
          )}
        </div>
      )}
    </div>
  ))}

</div>



      
      <Modal show={showServiziModal} onHide={() => setShowServiziModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Seleziona Servizi Aggiuntivi</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {servizi.map((servizio) => (
            <Form.Check
              key={servizio.id}
              type="checkbox"
              label={`${servizio.nome} (+${servizio.prezzo}€)`}
              onChange={() => toggleServizio(servizio)}
              checked={serviziSelezionati.includes(servizio)}
            />
          ))}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowServiziModal(false)}>
            Annulla
          </Button>
          <Button
            variant="primary"
            onClick={() => {
              calcolaTotale();
              setShowServiziModal(false);
              setShowConfermaModal(true);
            }}
          >
            Conferma Servizi
          </Button>
        </Modal.Footer>
      </Modal>

      
      <Modal show={showConfermaModal} onHide={() => setShowConfermaModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Conferma Prenotazione</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Il totale della prenotazione è: <strong>{totale}€</strong></p>
          <p>Sei sicuro di voler confermare la prenotazione?</p>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowConfermaModal(false)}>
            Annulla
          </Button>
          <Button variant="primary" onClick={confermaPrenotazione}>
            Conferma Prenotazione
          </Button>
        </Modal.Footer>
      </Modal>

     
      <Modal show={showProfiloModal} onHide={() => setShowProfiloModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Completa il tuo profilo</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>Prima di prenotare, completa il tuo profilo nella sezione dedicata.</p>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowProfiloModal(false)}>
            Chiudi
          </Button>
          <Button variant="primary" onClick={() => (window.location.href = '/profilo')}>
            Vai al Profilo
          </Button>
        </Modal.Footer>
      </Modal>
      
    </div>
    
    
  );


}

export default CamerePage;