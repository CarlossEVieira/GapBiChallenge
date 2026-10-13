
import { useEffect, useState } from 'react';
import api from './services/api';

// Importação dos componentes do dashboard
import GraficoCategorias from './components/GraficoCategorias';
import MaioresClientes from './components/MaioresClientes';

// Importação dos estilos
import './App.css';

function App() {

  // Armazena os indicadores recebidos da API
  const [indicadores, setIndicadores] = useState(null);

  // Armazena a data da última sincronização
  const [ultimaSincronizacao, setUltimaSincronizacao] = useState(null);

  // Controla a exibição do carregamento
  const [carregando, setCarregando] = useState(true);

  // Controla o botão durante a sincronização
  const [sincronizando, setSincronizando] = useState(false);

  // Armazena mensagens de erro ou sucesso
  const [mensagem, setMensagem] = useState('');

  // Formata valores para a moeda brasileira
  function formatarMoeda(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  // Formata a data UTC para o horário de Brasília
  function formatarData(data) {
    if (!data) {
      return 'Nenhuma sincronização realizada';
    }

    // Converte a data recebida da API para o horário brasileiro
    return new Date(data).toLocaleString('pt-BR', {
      timeZone: 'America/Sao_Paulo',
      dateStyle: 'short',
      timeStyle: 'medium'
    });
  }

  // Consulta os indicadores e a última sincronização
  async function carregarDados() {
    try {
      setCarregando(true);
      setMensagem('');

      // Executa as duas consultas ao mesmo tempo
      const [respostaIndicadores, respostaSincronizacao] =
        await Promise.all([
          api.get('/Indicadores'),
          api.get('/Sincronizacao/ultima')
        ]);

      // Atualiza os indicadores exibidos na tela
      setIndicadores(respostaIndicadores.data);

      // Atualiza a data da última sincronização
      setUltimaSincronizacao(
        respostaSincronizacao.data.ultimaSincronizacao
      );

    } catch (erro) {
      console.error('Erro ao carregar o dashboard:', erro);

      setMensagem(
        'Não foi possível carregar os dados da API.'
      );

    } finally {
      // Finaliza o carregamento mesmo que ocorra algum erro
      setCarregando(false);
    }
  }

  // Executa a sincronização completa dos dados
  async function sincronizarDados() {
    try {
      setSincronizando(true);
      setMensagem('');

      // Solicita ao backend a sincronização com o DummyJSON
      await api.post('/Sincronizacao/completa');

      // Atualiza os indicadores após a sincronização
      await carregarDados();

      // Exibe uma mensagem de sucesso
      setMensagem('Dados sincronizados com sucesso!');

    } catch (erro) {
      console.error('Erro na sincronização:', erro);

      setMensagem(
        'Ocorreu um erro ao sincronizar os dados.'
      );

    } finally {
      // Libera o botão após a conclusão
      setSincronizando(false);
    }
  }

  // Carrega os indicadores quando a página é aberta
  useEffect(() => {
    carregarDados();
  }, []);

  return (
    <div className="container py-4">

      {/* Cabeçalho principal do dashboard */}
      <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-4">

        <div>
          <h1 className="titulo-dashboard">
            GAP BI
          </h1>

          <p className="text-muted mb-0">
            Dashboard de indicadores de vendas
          </p>
        </div>

        {/* Botão para atualizar os dados */}
        <button
          className="btn btn-primary"
          onClick={sincronizarDados}
          disabled={sincronizando || carregando}
        >
          {sincronizando
            ? 'Sincronizando...'
            : 'Sincronizar dados'}
        </button>

      </div>

      {/* Data e hora da última sincronização */}
      <p className="text-muted mb-4">
        <strong>Última sincronização:</strong>{' '}
        {formatarData(ultimaSincronizacao)}
      </p>

      {/* Mensagens de sucesso ou erro */}
      {mensagem && (
        <div className="alert alert-info">
          {mensagem}
        </div>
      )}

      {/* Mensagem exibida durante o carregamento */}
      {carregando && (
        <div className="alert alert-secondary">
          Carregando indicadores...
        </div>
      )}

      {/* Conteúdo exibido após carregar os indicadores */}
      {!carregando && indicadores && (
        <>

          {/* Primeira seção: indicadores financeiros */}
          <div className="row g-3 mt-2">

            {/* Total de pedidos */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Total de pedidos</h6>

                  <h3>
                    {indicadores.numeroPedidos}
                  </h3>

                </div>
              </div>
            </div>

            {/* Faturamento bruto */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Faturamento bruto</h6>

                  <h3>
                    {formatarMoeda(
                      indicadores.faturamentoBruto
                    )}
                  </h3>

                </div>
              </div>
            </div>

            {/* Faturamento líquido */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Faturamento líquido</h6>

                  <h3>
                    {formatarMoeda(
                      indicadores.faturamentoLiquido
                    )}
                  </h3>

                </div>
              </div>
            </div>

            {/* Valor total de descontos */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Desconto concedido</h6>

                  <h3>
                    {formatarMoeda(
                      indicadores.descontoConcedido
                    )}
                  </h3>

                </div>
              </div>
            </div>

            {/* Percentual de desconto */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Percentual de desconto</h6>

                  <h3>
                    {Number(
                      indicadores.percentualDesconto
                    ).toFixed(2)}%
                  </h3>

                </div>
              </div>
            </div>

            {/* Ticket médio */}
            <div className="col-md-4">
              <div className="card card-indicador">
                <div className="card-body">

                  <h6>Ticket médio</h6>

                  <h3>
                    {formatarMoeda(
                      indicadores.ticketMedio
                    )}
                  </h3>

                </div>
              </div>
            </div>

          </div>

          {/* Segunda seção: gráfico de faturamento por categoria */}
          <GraficoCategorias
            categorias={indicadores.faturamentoPorCategoria}
          />

          {/* Terceira seção: ranking dos cinco maiores clientes */}
          <MaioresClientes
            clientes={indicadores.maioresClientes}
          />

        </>
      )}

    </div>
  );
}

export default App;
