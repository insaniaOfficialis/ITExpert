using Data.Models.Base.Request;
using Data.Models.Codes.Response;

namespace Services.Codes.GetCodeBD
{
    /// <summary>
    /// Интерфейс получения кодов
    /// </summary>
    public interface IGetCodesBD
    {
        /// <summary>
        /// Метод получения кодов
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<GetCodesRespose> Get(BaseSortRequest? sort, int? skip, int? take, int? orderNumber, int? code, string? value);
    }
}
