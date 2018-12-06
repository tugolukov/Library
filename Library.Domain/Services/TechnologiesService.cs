using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Library.Database;
using Library.Database.Models;
using Library.Domain.Interfaces;
using Library.Domain.Models.Technology;
using Microsoft.EntityFrameworkCore;

namespace Library.Domain.Services
{
    /// <inheritdoc />
    public class TechnologiesService : ITechnologiesService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        /// <inheritdoc />
        public TechnologiesService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Guid> Create(CreateTechnologyModel model)
        {
            var technology = _mapper.Map<Technology>(model);
            _context.Technologies.Add(technology);
            await _context.SaveChangesAsync();
            return technology.TechnologyGuid;
        }

        /// <inheritdoc />
        public async Task<List<TechnologyModel>> ReadAll()
        {
            var technologies = await _context.Technologies.ToListAsync();
            return _mapper.Map<List<Technology>, List<TechnologyModel>>(technologies);
        }

        /// <inheritdoc />
        public async Task<TechnologyModel> Read(Guid technologyGuid)
        {
            var technology = await _context.Technologies.FindAsync(technologyGuid);
            return _mapper.Map<TechnologyModel>(technology);
        }

        /// <inheritdoc />
        public async Task Update(UpdateTechnologyModel model)
        {
            var technology = await _context.Technologies.FindAsync(model.TechnologyGuid);
            technology = _mapper.Map<Technology>(model);

            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid technologyGuid)
        {
            var technology = await _context.Technologies.FindAsync(technologyGuid);
            _context.Technologies.Remove(technology);
        }

        /// <inheritdoc />
        public async Task<List<TechnologyModel>> Search(string search)
        {
            var technologies = await _context.Technologies
                .Where(technology => technology.Name.Contains(search) ||
                                     technology.Description.Contains(search)).ToListAsync();
            return _mapper.Map<List<Technology>, List<TechnologyModel>>(technologies);
        }
    }
}