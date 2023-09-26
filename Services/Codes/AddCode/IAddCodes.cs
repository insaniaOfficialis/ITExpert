using Data.Models.Base.Response;
using System.Text.Json.Nodes;

namespace Services.Codes.AddCode
{
    /// <summary>
    /// Интерфейс добавления кодов
    /// </summary>
    public interface IAddCodes
    {
        /// <summary>
        /// Метод добавления кодов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<BaseResponse> AddCode(JsonArray? request);
    }
}
