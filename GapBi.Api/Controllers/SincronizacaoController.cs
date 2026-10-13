
using GapBi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GapBi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SincronizacaoController : ControllerBase
    {
        private readonly SincronizacaoService sincronizacaoService;

        public SincronizacaoController(
            SincronizacaoService sincronizacaoService)
        {
            this.sincronizacaoService = sincronizacaoService;
        }

        // Endpoint para sincronizar os produtos
        [HttpPost("produtos")]
        public async Task<IActionResult> SincronizarProdutos()
        {
            try
            {
                int quantidade = await sincronizacaoService
                    .SincronizarProdutosAsync();

                return Ok(new
                {
                    mensagem = "Produtos sincronizados com sucesso!",
                    quantidadeProdutos = quantidade
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    mensagem = "Não foi possível acessar a API DummyJSON."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensagem = "Ocorreu um erro ao sincronizar os produtos."
                });
            }
        }
        
        // Endpoint para sincronizar os clientes
        [HttpPost("clientes")]
        public async Task<IActionResult> SincronizarClientes()
        {
            try
            {
                int quantidade = await sincronizacaoService
                    .SincronizarClientesAsync();

                return Ok(new
                {
                    mensagem = "Clientes sincronizados com sucesso!",
                    quantidadeClientes = quantidade
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    mensagem = "Não foi possível acessar a API DummyJSON."
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensagem = "Ocorreu um erro ao sincronizar os clientes."
                });
            }
        }
        
        // Endpoint para sincronizar os pedidos e seus itens
        [HttpPost("pedidos")]
        public async Task<IActionResult> SincronizarPedidos()
        {
            try
            {
                int quantidade = await sincronizacaoService
                    .SincronizarPedidosAsync();

                return Ok(new
                {
                    mensagem = "Pedidos sincronizados com sucesso!",
                    quantidadePedidos = quantidade
                });
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {
                    mensagem = "Não foi possível acessar a API DummyJSON."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensagem = "Erro ao sincronizar os pedidos.",
                    detalhe = ex.Message
                });
            }
        }
        
        // Endpoint para sincronizar produtos, clientes e pedidos
        [HttpPost("completa")]
        public async Task<IActionResult> SincronizarTudo()
        {
            try
            {
                var resultado = await sincronizacaoService
                    .SincronizarTudoAsync();

                return Ok(resultado);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensagem = "Não foi possível concluir a sincronização."
                });
            }
        }
        
        // Endpoint para consultar a última sincronização realizada
        [HttpGet("ultima")]
        public async Task<IActionResult> ObterUltimaSincronizacao()
        {
            try
            {
                // Consulta a data e hora da última sincronização
                var dataHora = await sincronizacaoService
                    .ObterUltimaSincronizacaoAsync();

                // Retorna uma mensagem caso ainda não exista sincronização
                if (dataHora == null)
                {
                    return Ok(new
                    {
                        mensagem = "Nenhuma sincronização realizada.",
                        ultimaSincronizacao = (DateTime?)null
                    });
                }

                // Retorna a data e hora em UTC
                return Ok(new
                {
                    mensagem = "Última sincronização consultada com sucesso.",
                    ultimaSincronizacao = dataHora
                });
            }
            catch (Exception)
            {
                // Retorna uma mensagem caso ocorra algum erro
                return StatusCode(500, new
                {
                    mensagem = "Erro ao consultar a última sincronização."
                });
            }
        }
    }
}
