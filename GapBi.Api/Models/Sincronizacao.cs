
namespace GapBi.Api.Models
{
    public class Sincronizacao
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        public bool Sucesso { get; set; }

        public string Mensagem { get; set; } = string.Empty;
    }
}
