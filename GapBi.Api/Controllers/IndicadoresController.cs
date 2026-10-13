
using GapBi.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GapBi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IndicadoresController : ControllerBase
    {
        private readonly IndicadoresService indicadoresService;

        public IndicadoresController(IndicadoresService indicadoresService)
        {
            this.indicadoresService = indicadoresService;
        }

        // Endpoint que retorna os indicadores do dashboard
        [HttpGet]
        public async Task<IActionResult> ObterIndicadores()
        {
            try
            {
                var indicadores = await indicadoresService
                    .ObterIndicadoresAsync();

                return Ok(indicadores);
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    mensagem = "Erro ao consultar os indicadores."
                });
            }
        }
    }
}
