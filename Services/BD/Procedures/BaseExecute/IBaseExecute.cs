namespace Services.BD.Procedures.BaseExecute
{
    /// <summary>
    /// Базовый интерфейс вызова хранимых процедур
    /// </summary>
    public interface IBaseExecute
    {
        /// <summary>
        /// Метод вызова хранимых процедур
        /// </summary>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        Task<string?> Execute(string name, string body);
    }
}
