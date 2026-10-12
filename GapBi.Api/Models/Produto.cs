
namespace GapBi.Api.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public int IdExterno { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public List<PedidoItem> PedidoItens { get; set; } = new();
    }
}
