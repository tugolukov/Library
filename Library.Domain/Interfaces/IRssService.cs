using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.RSS;

namespace Library.Domain.Interfaces
{
    public interface IRssService
    {
        /// <summary>
        /// Получение всех новостей для отображения во вкладках
        /// </summary>
        /// <returns></returns>
        Task<List<RssGroupModel>> GetSources();

        /// <summary>
        /// Добавление источника из URL
        /// </summary>
        /// <param name="uri">URL источника</param>
        /// <returns></returns>
        Task<Guid> AddSourceWithUrl(string uri);

        /// <summary>
        /// Добавление своего канала
        /// </summary>
        /// <param name="name">Название канала</param>
        /// <returns></returns>
        Task<Guid> AddSourceWithoutUrl(string name);

        /// <summary>
        /// Добавление новости
        /// </summary>
        /// <param name="item">Новость</param>
        /// <returns></returns>
        Task<Guid> AddItem(RssItemModel item);
    }
}