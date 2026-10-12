
namespace GapBi.Api.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        public int IdExterno { get; set; }

        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;

        public decimal Total { get; set; }

        public decimal TotalComDesconto { get; set; }

        public List<PedidoItem> Itens { get; set; } = new();
    }
}
