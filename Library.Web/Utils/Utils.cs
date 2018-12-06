using System;
using System.Collections.Generic;
using System.Linq;
using Library.Domain.Models.Author;
using Library.Domain.Models.Book;
using Library.Domain.Models.Publishing;
using Library.Domain.Models.Technology;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Web.Utils
{
    public static class Utils
    {
        public static List<SelectListItem> ToSelectListItems(this List<AuthorModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var author in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{author.Surname} {author.Name}",
                    Value = author.AuthorGuid.ToString()
                };
                result.Add(item);
            }

            return result;
        }

        public static List<SelectListItem> ToSelectListItems(this List<TechnologyModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var technology in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{technology.Name}",
                    Value = technology.TechnologyGuid.ToString()
                };
                result.Add(item);
            }

            return result;
        }

        public static List<SelectListItem> ToSelectListItems(this List<PublishingModel> items)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (var publishing in items)
            {
                SelectListItem item = new SelectListItem()
                {
                    Text = $"{publishing.Name}",
                    Value = publishing.PublishingGuid.ToString()
                };
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Получение описания для книги
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetDescriptionTags(this BookModel model)
        {
            var authors = model.GetAuthorsInfo();
            var publishing = model.GetPublishingsInfo();
            var technologies = model.GetTechnologiesInfo();
            var annotation = model.GetAnnotation();
            var title = model.GetTitle();
            
            var info = new List<string>();
            info.AddRange(title);
            info.AddRange(annotation);
            info.AddRange(authors);
            info.AddRange(publishing);
            info.AddRange(technologies);

            return info.ToStringFromList();
        }

        /// <summary>
        /// Получение названия для поиска
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetTitle(this BookModel model)
        {
            List<string> result = new List<string>();
            result.AddRange(model.Title.Split(new[] {' ', '.', ',', ';', '-'}));
            return result;
        }

        /// <summary>
        /// Получение аннотации для поиска
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetAnnotation(this BookModel model)
        {
            List<string> result = new List<string>();
            result.AddRange(model.Annotation.Split(new[] {' ', '.', ',', ';', '-'}));
            return result;
        }

        /// <summary>
        /// Получение списка данных об авторах для поиска
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetAuthorsInfo(this BookModel model)
        {
            List<string> authors = new List<string>();
            foreach (var author in model.AuthorModel)
            {
                authors.AddRange(author.Note.Split(new char[] {' ', ',', ';', '.'}).ToList());
                if (string.IsNullOrEmpty(author.Name))
                {
                    authors.Add(author.Name);
                }

                if (string.IsNullOrEmpty(author.Surname))
                {
                    authors.Add(author.Surname);
                }

                if (string.IsNullOrEmpty(author.Patronymic))
                {
                    authors.Add(author.Patronymic);
                }
            }

            return authors.Distinct();
        }

        /// <summary>
        /// Получение списка данных об издательстве для поиска
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetPublishingsInfo(this BookModel model)
        {
            List<string> info = new List<string>();
            info.AddRange(new[]
            {
                model.PublishingModel.Name,
                model.PublishingModel.City,
                model.PublishingModel.Country,
                model.PublishingModel.House,
                model.PublishingModel.State,
                model.PublishingModel.Street,
                model.PublishingModel.Postcode.ToString()
            });

            return info;
        }

        /// <summary>
        /// Получение списка данных о серии для поиска
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetTechnologiesInfo(this BookModel model)
        {
            List<string> result = new List<string>();
            result.AddRange(model.TechnologyModel.Name.Split(' '));
            result.AddRange(model.TechnologyModel.Description.Split(' ', ',', '.'));
            result.AddRange(model.TechnologyModel.Language.Split(' '));
            return result;
        }

        /// <summary>
        /// Получить строку, разделенную пробелами из списка
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        private static string ToStringFromList(this List<string> strings)
        {
            return String.Join(' ', strings);
        }
    }
}