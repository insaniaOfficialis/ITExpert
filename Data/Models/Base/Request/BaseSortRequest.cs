namespace Data.Models.Base.Request
{
    public class BaseSortRequest
    {
        /// <summary>
        /// Поле для сортировки
        /// </summary>
        public string? SortKey { get; set; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public bool? IsAscending { get; set; }
    }
}
