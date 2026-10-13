
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer
} from 'recharts';

// Componente responsável pelo gráfico de faturamento por categoria
function GraficoCategorias({ categorias }) {

  // Formata os valores monetários em reais
  function formatarMoeda(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  // Organiza as categorias da maior receita para a menor
  const categoriasOrdenadas = [...(categorias ?? [])].sort(
    (primeira, segunda) =>
      segunda.faturamentoLiquido - primeira.faturamentoLiquido
  );

  return (
    <div className="card card-indicador mt-4">
      <div className="card-body">
        <h5 className="mb-3">Faturamento por categoria</h5>

        {categoriasOrdenadas.length === 0 ? (
          <p className="text-muted">
            Nenhuma categoria encontrada.
          </p>
        ) : (
          <div style={{ width: '100%', height: 650 }}>
            <ResponsiveContainer width="100%" height="100%">
              <BarChart
                data={categoriasOrdenadas}
                layout="vertical"
                margin={{
                  top: 10,
                  right: 35,
                  left: 45,
                  bottom: 10
                }}
              >
                {/* Linhas de referência do gráfico */}
                <CartesianGrid
                  strokeDasharray="3 3"
                  horizontal={false}
                />

                {/* Eixo horizontal: valores monetários */}
                <XAxis
                  type="number"
                  tickFormatter={(valor) =>
                    `R$ ${(valor / 1000).toFixed(0)} mil`
                  }
                />

                {/* Eixo vertical: nomes das categorias */}
                <YAxis
                  dataKey="categoria"
                  type="category"
                  width={130}
                  tick={{ fontSize: 11 }}
                />

                {/* Mostra os valores ao passar o mouse */}
                <Tooltip
                  formatter={(valor) => [
                    formatarMoeda(valor),
                    'Faturamento líquido'
                  ]}
                />

                {/* Barras de faturamento */}
                <Bar
                  dataKey="faturamentoLiquido"
                  fill="#17365d"
                  radius={[0, 5, 5, 0]}
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        )}
      </div>
    </div>
  );
}

export default GraficoCategorias;
