
using GapBi.Api.Data;
using GapBi.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GapBi.Api.Services
{
    public class IndicadoresService
    {
        private readonly AppDbContext contextoBancoDados;

        public IndicadoresService(AppDbContext contextoBancoDados)
        {
            this.contextoBancoDados = contextoBancoDados;
        }

        // Calcula os indicadores utilizando os dados do SQL Server
        public async Task<IndicadoresDashboardDto> ObterIndicadoresAsync()
        {
            // Calcula a quantidade total de pedidos
            int numeroPedidos = await contextoBancoDados.Pedidos.CountAsync();

            // Calcula o faturamento bruto dos pedidos
            decimal faturamentoBruto = await contextoBancoDados.Pedidos
                .SumAsync(pedido => pedido.Total);

            // Calcula o faturamento líquido dos pedidos
            decimal faturamentoLiquido = await contextoBancoDados.Pedidos
                .SumAsync(pedido => pedido.TotalComDesconto);

            // Calcula o desconto concedido em valor
            decimal descontoConcedido = faturamentoBruto - faturamentoLiquido;

            // Calcula o percentual de desconto
            decimal percentualDesconto = faturamentoBruto > 0
                ? descontoConcedido / faturamentoBruto * 100
                : 0;

            // Calcula o ticket médio dos pedidos
            decimal ticketMedio = numeroPedidos > 0
                ? faturamentoLiquido / numeroPedidos
                : 0;

            // Calcula o faturamento líquido agrupado por categoria
            var faturamentoPorCategoria = await contextoBancoDados.PedidoItens
                .GroupBy(item => item.Produto.Categoria)
                .Select(grupo => new FaturamentoCategoriaDto
                {
                    Categoria = grupo.Key,
                    FaturamentoLiquido = grupo.Sum(
                        item => item.TotalComDesconto
                    )
                })
                .OrderByDescending(categoria => categoria.FaturamentoLiquido)
                .ToListAsync();

            // Busca os cinco clientes com maior faturamento líquido
            var maioresClientes = await contextoBancoDados.Pedidos
                .GroupBy(pedido => new
                {
                    pedido.ClienteId,
                    pedido.Cliente.Nome,
                    pedido.Cliente.Sobrenome
                })
                .Select(grupo => new ClienteFaturamentoDto
                {
                    NomeCompleto = grupo.Key.Nome + " " + grupo.Key.Sobrenome,
                    FaturamentoLiquido = grupo.Sum(
                        pedido => pedido.TotalComDesconto
                    )
                })
                .OrderByDescending(cliente => cliente.FaturamentoLiquido)
                .Take(5)
                .ToListAsync();

            // Organiza os indicadores para enviar ao frontend
            return new IndicadoresDashboardDto
            {
                NumeroPedidos = numeroPedidos,
                FaturamentoBruto = faturamentoBruto,
                FaturamentoLiquido = faturamentoLiquido,
                DescontoConcedido = descontoConcedido,
                PercentualDesconto = Math.Round(percentualDesconto, 2),
                TicketMedio = Math.Round(ticketMedio, 2),
                FaturamentoPorCategoria = faturamentoPorCategoria,
                MaioresClientes = maioresClientes
            };
        }
    }
}
