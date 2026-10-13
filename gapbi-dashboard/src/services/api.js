
import axios from 'axios';

// Configuração da comunicação com nossa API .NET
const api = axios.create({
  baseURL: 'http://localhost:5158/api',
  timeout: 30000
});

export default api;
