using System.Threading.Tasks;
using Library.Domain.Models.Visitor;

namespace Library.Domain.Interfaces
{
    public interface IVisitorsService
    {
        Task<CounterModel> GetCounter();    
    }
}