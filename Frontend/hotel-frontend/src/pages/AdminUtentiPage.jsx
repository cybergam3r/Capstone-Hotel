import React, { useEffect, useState } from 'react';
import api from '../services/api';
import { toast } from 'react-toastify';
import { useSelector } from 'react-redux';

function AdminUtentiPage() {
  const [utenti, setUtenti] = useState([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 5;

  const currentUserEmail = useSelector((state) => state.auth.user?.email);

  const loadUtenti = async () => {
    try {
      const res = await api.get('/user');
      setUtenti(res.data);
    } catch {
      toast.error('Errore nel caricamento utenti');
    } finally {
      setLoading(false);
    }
  };

  const cambiaRuolo = async (id, nuovoRuolo) => {
    try {
      await api.put(`/user/${id}/role`, nuovoRuolo, {
        headers: { 'Content-Type': 'application/json' },
      });
      toast.success('Ruolo aggiornato');
      loadUtenti();
    } catch {
      toast.error('Errore aggiornamento ruolo');
    }
  };

  const eliminaUtente = async (id, email) => {
    if (email === currentUserEmail) {
      toast.warning("Non puoi eliminare te stesso.");
      return;
    }

    if (!window.confirm(`Sei sicuro di voler eliminare ${email}?`)) return;

    try {
      await api.delete(`/user/${id}`);
      toast.success('Utente eliminato');
      loadUtenti();
    } catch {
      toast.error('Errore durante l\'eliminazione');
    }
  };

  useEffect(() => {
    loadUtenti();
  }, []);

  const utentiFiltrati = utenti.filter((u) =>
    u.email.toLowerCase().includes(search.toLowerCase())
  );

  
  const totalPages = Math.ceil(utentiFiltrati.length / pageSize);
  const start = (currentPage - 1) * pageSize;
  const utentiPagina = utentiFiltrati.slice(start, start + pageSize);

  const paginaPrecedente = () => {
    if (currentPage > 1) setCurrentPage(currentPage - 1);
  };

  const paginaSuccessiva = () => {
    if (currentPage < totalPages) setCurrentPage(currentPage + 1);
  };

  if (loading) return <div className="container mt-4">Caricamento utenti...</div>;

  return (
    <div className="container mt-4">
      <h3>Gestione Utenti</h3>

      <div className="mb-3">
        <input
          type="text"
          className="form-control"
          placeholder="Cerca per email..."
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setCurrentPage(1); 
          }}
        />
      </div>

      {utentiPagina.length === 0 ? (
        <div className="alert alert-info">Nessun utente trovato.</div>
      ) : (
        <>
          <table className="table table-bordered table-striped mt-3">
            <thead className="table-dark">
              <tr>
                <th>Email</th>
                <th>Ruolo</th>
                <th>Azioni</th>
              </tr>
            </thead>
            <tbody>
              {utentiPagina.map((u) => (
                <tr key={u.id}>
                  <td>{u.email}</td>
                  <td>{u.roles[0]}</td>
                  <td>
                    {u.email === currentUserEmail ? (
                      <span className="text-muted">Non modificabile</span>
                    ) : (
                      <>
                        {u.roles[0] === 'User' ? (
                          <button
                            className="btn btn-sm btn-warning me-2"
                            onClick={() => cambiaRuolo(u.id, 'Admin')}
                          >
                            Promuovi a Admin
                          </button>
                        ) : (
                          <button
                            className="btn btn-sm btn-secondary me-2"
                            onClick={() => cambiaRuolo(u.id, 'User')}
                          >
                            Declassa a User
                          </button>
                        )}
                        <button
                          className="btn btn-sm btn-danger"
                          onClick={() => eliminaUtente(u.id, u.email)}
                        >
                          Elimina
                        </button>
                      </>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

         
          <div className="d-flex justify-content-between align-items-center mt-3">
            <button
              className="btn btn-outline-primary"
              disabled={currentPage === 1}
              onClick={paginaPrecedente}
            >
              ← Pagina precedente
            </button>

            <span>
              Pagina {currentPage} di {totalPages}
            </span>

            <button
              className="btn btn-outline-primary"
              disabled={currentPage === totalPages}
              onClick={paginaSuccessiva}
            >
              Pagina successiva →
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default AdminUtentiPage;
