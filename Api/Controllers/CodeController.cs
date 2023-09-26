using Data.Models.Base.Request;
using Data.Models.Base.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Codes.AddCode;
using Services.Codes.GetCode;
using Services.Codes.GetCodeBD;
using System.Text.Json.Nodes;

namespace Api.Controllers
{
    /// <summary>
    /// Контроллер кодов
    /// </summary>
    [Route("api/v1/codes")]
    public class CodeController : Controller
    {
        private readonly ILogger<CodeController> _logger; //интерфейс для записи логов
        private readonly IAddCodes _addCodes; //интерфейс добавления кодов
        private readonly IGetCodes _getCodes; //интерфейс получения кодов
        private readonly IGetCodesBD _getCodesBD; //интерфейс получения из бд

        /// <summary>
        /// Конструктор контроллера кодов
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="addCodes"></param>
        /// <param name="getCodes"></param>
        /// <param name="getCodesBD"></param>
        public CodeController(ILogger<CodeController> logger, IAddCodes addCodes, IGetCodes getCodes, IGetCodesBD getCodesBD)
        {
            _logger = logger;
            _addCodes = addCodes;
            _getCodes = getCodes;
            _getCodesBD = getCodesBD;
        }

        [HttpPost]
        public async Task<IActionResult> AddCode([FromBody] JsonArray? request)
        {
            try
            {
                var result = await _addCodes.AddCode(request);

                if(result.Success)
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
                        _logger.LogError("AddCode. Непредвиденная ошибка");
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
        public async Task<IActionResult> GetCode([FromQuery] List<BaseSortRequest?>? sort)
        {
            try
            {
                var result = await _getCodes.Get(sort);

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
                        _logger.LogError("GetCode. Непредвиденная ошибка");
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
        public async Task<IActionResult> GetCode([FromQuery] BaseSortRequest? sort)
        {
            try
            {
                var result = await _getCodesBD.Get(sort);

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
                        _logger.LogError("GetCode. Непредвиденная ошибка");
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
