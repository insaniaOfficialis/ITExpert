using Data.Models.Base.Request;
using Data.Models.Codes.Response;

namespace Services.Codes.GetCode
{
    /// <summary>
    /// Интерфейс получения кодов
    /// </summary>
    public interface IGetCodes
    {
        /// <summary>
        /// Метод получения кодов
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        Task<GetCodesRespose> Get(List<BaseSortRequest?>? sort);
    }
}
