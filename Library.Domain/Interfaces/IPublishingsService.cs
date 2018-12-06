using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Publishing;

namespace Library.Domain.Interfaces
{
    public interface IPublishingsService
    {
        /// <summary>
        /// Создание 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Create(CreatePublishingModel model);

        /// <summary>
        /// Получить список 
        /// </summary>
        /// <returns></returns>
        Task<List<PublishingModel>> ReadAll();

        /// <summary>
        /// Получить книгу по идентификатору
        /// </summary>
        /// <param name="publishingGuid"></param>
        /// <returns></returns>
        Task<PublishingModel> Read(Guid publishingGuid);

        /// <summary>
        /// Редактировать 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdatePublishingModel model);

        /// <summary>
        /// Удалить книгу по идентификатору
        /// </summary>
        /// <param name="publishingGuid"></param>
        /// <returns></returns>
        Task Delete(Guid publishingGuid);
        
        /// <summary>
        /// Поиск издательства
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<PublishingModel>> Search(string search);
    }
}