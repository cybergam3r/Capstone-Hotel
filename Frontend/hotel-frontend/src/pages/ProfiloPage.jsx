import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { Form, Button } from 'react-bootstrap';
import { toast } from 'react-toastify';
import '../App.css'; 

function ProfiloPage() {
  const [profilo, setProfilo] = useState({
    nome: '',
    cognome: '',
    codiceFiscale: '',
    dataDiNascita: '',
    numeroDiTelefono: '',
  });

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProfilo = async () => {
      try {
        const response = await api.get('/account/profilo');
        setProfilo(response.data);
      } catch (error) {
        toast.error('Errore nel caricamento del profilo.');
        console.error(error);
      } finally {
        setLoading(false);
      }
    };

    fetchProfilo();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      await api.put('/account/profilo', profilo);
      toast.success('Profilo aggiornato con successo!');
    } catch (error) {
      toast.error('Errore durante l\'aggiornamento del profilo.');
      console.error(error);
    }
  };

  if (loading) {
    return <div className="container mt-4">Caricamento profilo...</div>;
  }

  return (
    <div className="profilo-page">
      <div className="profilo-container">
        <h3 className="text-center mb-4">Profilo Utente</h3>
        <Form onSubmit={handleSubmit} className="profilo-form">
          <Form.Group className="mb-3">
            <Form.Label>Nome</Form.Label>
            <Form.Control
              type="text"
              value={profilo.nome}
              onChange={(e) => setProfilo({ ...profilo, nome: e.target.value })}
              required
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Cognome</Form.Label>
            <Form.Control
              type="text"
              value={profilo.cognome}
              onChange={(e) => setProfilo({ ...profilo, cognome: e.target.value })}
              required
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Codice Fiscale</Form.Label>
            <Form.Control
              type="text"
              value={profilo.codiceFiscale}
              onChange={(e) => setProfilo({ ...profilo, codiceFiscale: e.target.value })}
              required
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Data di Nascita</Form.Label>
            <Form.Control
              type="date"
              value={profilo.dataDiNascita ? profilo.dataDiNascita.split('T')[0] : ''}
              onChange={(e) => setProfilo({ ...profilo, dataDiNascita: e.target.value })}
              required
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Numero di Telefono</Form.Label>
            <Form.Control
              type="text"
              value={profilo.numeroDiTelefono}
              onChange={(e) => setProfilo({ ...profilo, numeroDiTelefono: e.target.value })}
              required
            />
          </Form.Group>
          <Button variant="primary" type="submit" className="w-100">
            Salva Profilo
          </Button>
        </Form>
      </div>
    </div>
  );
}

export default ProfiloPage;