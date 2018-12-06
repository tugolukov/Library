using System;
using System.Threading.Tasks;
using AutoMapper;
using Library.Database;
using Library.Database.Models.Visitor;
using Library.Domain.Interfaces;
using Library.Domain.Models.Visitor;

namespace Library.Domain.Services
{
    public class VisitorsService : IVisitorsService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public VisitorsService(IMapper mapper, DatabaseContext context)
        {
            _mapper = mapper;
            _context = context;
        }


        public Task<CounterModel> GetCounter()
        {            
            throw new NotImplementedException();
        }
        
        private async Task AddVisitor(CreateVisitorModel visitorModel)
        {
            var visitor = _mapper.Map<Visitor>(visitorModel);
            visitor.VisitorGuid = Guid.NewGuid();

            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();
        }
    }
}
