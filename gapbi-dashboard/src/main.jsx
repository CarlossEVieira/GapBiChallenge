
import React from 'react';
import ReactDOM from 'react-dom/client';

// Importação do Bootstrap para estilização
import 'bootstrap/dist/css/bootstrap.min.css';

// Importação dos arquivos principais da aplicação
import App from './App.jsx';
import './index.css';

// Inicialização da aplicação React
ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
