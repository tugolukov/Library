using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Database;
using Library.Database.Models;
using Library.Domain.Interfaces;
using Library.Domain.Models.Author;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class AuthorsService : IAuthorsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public AuthorsService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Guid> Create(CreateAuthorModel model)
        {
            var author = _mapper.Map<Author>(model);
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return author.AuthorGuid;
        }

        /// <inheritdoc />
        public async Task<List<AuthorModel>> ReadAll()
        {
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<List<Author>, List<AuthorModel>>(authors);        
        }

        /// <inheritdoc />
        public async Task<AuthorModel> Read(Guid authorGuid)
        {
            var author = await _context.Authors.FindAsync(authorGuid);
            return _mapper.Map<AuthorModel>(author);
        }

        /// <inheritdoc />
        public async Task Update(UpdateAuthorModel model)
        {
            var author = await _context.Authors.FindAsync(model.AuthorGuid);
            author = _mapper.Map<Author>(model);

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid authorGuid)
        {
            var author = await _context.Authors.FindAsync(authorGuid);
            _context.Authors.Remove(author);
        }

        /// <inheritdoc />
        public async Task<List<AuthorModel>> Search(string search)
        {
            var strings = search.Split(new [] {' ', ';', ','});
            var results = new List<Author>();
            foreach (var str in strings)
            {
                var result = await _context.Authors
                    .Where(a => a.Surname.ToUpper().Contains(str.ToUpper()) ||
                                a.Name.ToUpper().Contains(str.ToUpper()) ||
                                a.Patronymic.ToUpper().Contains(str.ToUpper())).ToListAsync();
                results.AddRange(result);
            }

            results = results.Distinct().ToList();
            
            return _mapper.Map<List<Author>, List<AuthorModel>>(results);        
        }
    }
}