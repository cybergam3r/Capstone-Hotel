import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

// Redux & Redux Persist
import { Provider } from 'react-redux';
import { store, persistor } from './redux/store';
import { PersistGate } from 'redux-persist/integration/react';

// Stili globali
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-toastify/dist/ReactToastify.css';

// Notifiche
import { ToastContainer } from 'react-toastify';

// Avvia app
const root = ReactDOM.createRoot(document.getElementById('root'));

root.render(
  <React.StrictMode>
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <App />
        <ToastContainer position="top-center" autoClose={3000} />
      </PersistGate>
    </Provider>
  </React.StrictMode>
);