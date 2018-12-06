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
        Task<List<RssGroupModel>> GetAllGroups();

        /// <summary>
        /// Получение всех публикаций
        /// </summary>
        /// <returns></returns>
        Task<List<RssItemModel>> GetAll();

        /// <summary>
        /// Добавление источника
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<Guid> AddSource(string uri);

        /// <summary>
        /// Добавление источника
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<Guid> AddMySource(string name);
        
        /// <summary>
        /// Добавление новости
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<Guid> AddItem(RssItemModel item);
        
        /// <summary>
        /// Обновить публикациии из источников
        /// </summary>
        /// <returns></returns>
        Task Update();
    }
}