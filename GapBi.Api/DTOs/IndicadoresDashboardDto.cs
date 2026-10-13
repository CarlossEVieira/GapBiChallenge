
namespace GapBi.Api.DTOs
{
    public class IndicadoresDashboardDto
    {
        public int NumeroPedidos { get; set; }

        public decimal FaturamentoBruto { get; set; }

        public decimal FaturamentoLiquido { get; set; }

        public decimal DescontoConcedido { get; set; }

        public decimal PercentualDesconto { get; set; }

        public decimal TicketMedio { get; set; }

        public List<FaturamentoCategoriaDto> FaturamentoPorCategoria { get; set; } = new();

        public List<ClienteFaturamentoDto> MaioresClientes { get; set; } = new();
    }

    public class FaturamentoCategoriaDto
    {
        public string Categoria { get; set; } = string.Empty;

        public decimal FaturamentoLiquido { get; set; }
    }

    public class ClienteFaturamentoDto
    {
        public string NomeCompleto { get; set; } = string.Empty;

        public decimal FaturamentoLiquido { get; set; }
    }
}
