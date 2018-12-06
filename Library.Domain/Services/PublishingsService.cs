using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Database;
using Library.Database.Models;
using Library.Domain.Interfaces;
using Library.Domain.Models.Publishing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class PublishingsService : IPublishingsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public PublishingsService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Guid> Create(CreatePublishingModel model)
        {
            var publishing = _mapper.Map<Publishing>(model);
            _context.Publishings.Add(publishing);
            await _context.SaveChangesAsync();
            return publishing.PublishingGuid;
        }

        /// <inheritdoc />
        public async Task<List<PublishingModel>> ReadAll()
        {
            var publishings = await _context.Publishings.ToListAsync();
            return _mapper.Map<List<Publishing>, List<PublishingModel>>(publishings);
        }

        /// <inheritdoc />
        public async Task<PublishingModel> Read(Guid publishingGuid)
        {
            var publishing = await _context.Publishings.FindAsync(publishingGuid);
            return _mapper.Map<PublishingModel>(publishing);
        }

        /// <inheritdoc />
        public async Task Update(UpdatePublishingModel model)
        {
            var publishing = await _context.Publishings.FindAsync(model.PublishingGuid);
            publishing = _mapper.Map<Publishing>(model);

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid publishingGuid)
        {
            var publishing = await _context.Publishings.FindAsync(publishingGuid);
            _context.Publishings.Remove(publishing);
        }

        /// <inheritdoc />
        public async Task<List<PublishingModel>> Search(string search)
        {
            var strings = search.Split(new [] {' ', ';', ','});
            var results = new List<Publishing>();

            foreach (var str in strings)
            {
                var result = await _context.Publishings
                    .Where(publishing => publishing.Name.ToUpper().Contains(search.ToUpper())).ToListAsync();
                results.AddRange(result);
            }

            results = results.Distinct().ToList();
            
            return _mapper.Map<List<Publishing>, List<PublishingModel>>(results);
        }
    }
}