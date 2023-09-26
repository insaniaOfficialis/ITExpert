using Data.Models.Base.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Clients.GetClient;
using Services.Clients.GetClientBD;

namespace Api.Controllers
{
    /// <summary>
    /// Контроллер клиентов
    /// </summary>
    [Route("api/v1/clients")]
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger; //интерфейс для записи логов
        private readonly IGetClients _getClients; //интерфейс получения клиентов
        private readonly IGetClientsBD _getClientsBD; //интерфейс получения клиентов бд

        /// <summary>
        /// Конструктор контроллера кодов
        /// </summary>
        /// <param name="logger"></param>
        public ClientController(ILogger<ClientController> logger, IGetClients getClients, IGetClientsBD getClientsBD)
        {
            _logger = logger;
            _getClients = getClients;
            _getClientsBD = getClientsBD;
        }

        [HttpGet]
        public async Task<IActionResult> GetClients([FromQuery] int? count)
        {
            try
            {
                var result = await _getClients.Get(count);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    if (result != null && result.Error != null)
                    {
                        return StatusCode(result.Error.Code ?? 500, result);
                    }
                    else
                    {
                        _logger.LogError("GetClients. Непредвиденная ошибка");
                        BaseResponse response = new(false, new BaseError(500, "Непредвиденная ошибка"));
                        return StatusCode(500, response);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse(false, new BaseError(500, ex.Message)));
            }
        }

        [HttpGet]
        [Route("Bd")]
        public async Task<IActionResult> GetClientsBD([FromQuery] int? count)
        {
            try
            {
                var result = await _getClientsBD.Get(count);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    if (result != null && result.Error != null)
                    {
                        return StatusCode(result.Error.Code ?? 500, result);
                    }
                    else
                    {
                        _logger.LogError("GetClientsBD. Непредвиденная ошибка");
                        BaseResponse response = new(false, new BaseError(500, "Непредвиденная ошибка"));
                        return StatusCode(500, response);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse(false, new BaseError(500, ex.Message)));
            }
        }
    }
}
