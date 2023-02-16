using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Domain.Entities.Studies;

namespace ParticipantApi.Acceptance.Tests.Stubs
{
    public class StudyRepositoryStub : IStudyRepository
    {
        private readonly ConcurrentBag<Study> _studies;

        public StudyRepositoryStub()
        {
            _studies = new ConcurrentBag<Study>();
        }
        
        public async Task<Study> GetStudyAsync(long studyId)
        {
            var study = _studies.FirstOrDefault(x => x.StudyId == studyId);
            
            return await Task.FromResult(study);
        }
    }
}