using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Technology;

namespace Library.Domain.Interfaces
{
    /// <summary>
    /// Сервис работы со сферами применения
    /// </summary>
    public interface ITechnologiesService
    {
        /// <summary>
        /// Создание 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Create(CreateTechnologyModel model);

        /// <summary>
        /// Получить список 
        /// </summary>
        /// <returns></returns>
        Task<List<TechnologyModel>> ReadAll();

        /// <summary>
        /// Получить книгу по идентификатору
        /// </summary>
        /// <param name="technologyGuid"></param>
        /// <returns></returns>
        Task<TechnologyModel> Read(Guid technologyGuid);

        /// <summary>
        /// Редактировать 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdateTechnologyModel model);

        /// <summary>
        /// Удалить книгу по идентификатору
        /// </summary>
        /// <param name="technologyGuid"></param>
        /// <returns></returns>
        Task Delete(Guid technologyGuid);
        
        /// <summary>
        /// Поиск сферы деятельности
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<TechnologyModel>> Search(string search);
    }
}