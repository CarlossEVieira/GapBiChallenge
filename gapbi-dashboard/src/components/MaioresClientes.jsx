
// Componente responsável pelo ranking dos cinco maiores clientes
function MaioresClientes({ clientes }) {

  // Formata os valores monetários em reais
  function formatarMoeda(valor) {
    return Number(valor ?? 0).toLocaleString('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    });
  }

  return (
    <div className="card card-indicador mt-4">
      <div className="card-body">

        <h5 className="mb-3">
          Top 5 clientes por faturamento
        </h5>

        {!clientes || clientes.length === 0 ? (
          <p className="text-muted">
            Nenhum cliente encontrado.
          </p>
        ) : (
          <div className="table-responsive">
            <table className="table table-hover align-middle">
              <thead>
                <tr>
                  <th>Posição</th>
                  <th>Cliente</th>
                  <th className="text-end">
                    Faturamento líquido
                  </th>
                </tr>
              </thead>

              <tbody>
                {clientes.map((cliente, indice) => (
                  <tr key={`${cliente.nomeCompleto}-${indice}`}>
                    <td>{indice + 1}º</td>

                    <td>{cliente.nomeCompleto}</td>

                    <td className="text-end fw-semibold">
                      {formatarMoeda(cliente.faturamentoLiquido)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}

      </div>
    </div>
  );
}

export default MaioresClientes;
