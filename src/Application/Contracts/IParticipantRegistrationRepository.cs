using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.ParticipantRegistrations;

namespace Application.Contracts
{
    public interface IParticipantRegistrationRepository
    {
        Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteAsync(long studyId, string siteId);
        Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudyAsync(long studyId, string participantId);
        Task<ParticipantRegistration> GetParticipantByStudySiteAsync(long studyId, string siteId, string participantId);
        Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteStatusAsync(long studyId, string siteId, ParticipantRegistrationStatus participantRegistrationStatus);
        Task<IEnumerable<ParticipantRegistration>> GetParticipantRegistrationsStatusByStudy(long studyId, ParticipantRegistrationStatus participantRegistrationStatus);
        Task CreateParticipantRegistrationAsync(ParticipantRegistration entity);
        Task SaveParticipantRegistrationAsync(ParticipantRegistration entity);
    }
}