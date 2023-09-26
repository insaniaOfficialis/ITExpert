using Data.Models.Base.Response;
using CodesEntity = Data.Entites.Codes;

namespace Data.Models.Codes.Response
{
    /// <summary>
    /// Модель ответа за списком кодов
    /// </summary>
    public class GetCodesRespose : BaseResponse
    {
        /// <summary>
        /// Простой конструктор ответа за списком кодов
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        public GetCodesRespose(bool success, BaseError? error) : base(success, error)
        {

        }

        /// <summary>
        /// Конструктор модели ответа за списком кодов
        /// </summary>
        /// <param name="success"></param>
        /// <param name="error"></param>
        /// <param name="items"></param>
        public GetCodesRespose(bool success, BaseError? error, List<CodesEntity>? items) : base(success, error)
        {
            Items = items;
        }

        public List<CodesEntity>? Items { get; set; }
    }
}
