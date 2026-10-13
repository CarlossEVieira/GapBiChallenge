
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

    }
}
