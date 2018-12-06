using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models.Author;

namespace Library.Domain.Interfaces
{
    /// <summary>
    /// Сервис по работе с авторами
    /// </summary>
    public interface IAuthorsService
    {
        /// <summary>
        /// Создание автора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Guid> Create(CreateAuthorModel model);

        /// <summary>
        /// Получить список авторов
        /// </summary>
        /// <returns></returns>
        Task<List<AuthorModel>> ReadAll();

        /// <summary>
        /// Получить автора по идентификатору
        /// </summary>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
        Task<AuthorModel> Read(Guid bookGuid);

        /// <summary>
        /// Редактировать автора
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task Update(UpdateAuthorModel model);

        /// <summary>
        /// Удалить автора по идентификатору
        /// </summary>
        /// <param name="bookGuid"></param>
        /// <returns></returns>
        Task Delete(Guid bookGuid);

        /// <summary>
        /// Поиск автора
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<AuthorModel>> Search(string search);
    }
}