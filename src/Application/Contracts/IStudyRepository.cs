using System.Threading.Tasks;
using Domain.Entities.Studies;

namespace Application.Contracts
{
    public interface IStudyRepository
    {
        Task<Study> GetStudyAsync(long studyId);
    }
}