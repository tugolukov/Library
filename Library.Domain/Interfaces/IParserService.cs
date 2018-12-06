using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    /// <summary>
    /// Парсеры
    /// </summary>
    public interface IParserService
    {
        /// <summary>
        /// Парсинг сайта издательства ПИТЕР
        /// </summary>
        /// <returns></returns>
        Task ParsePiter();

        Task ParseEksmo();
    }
}