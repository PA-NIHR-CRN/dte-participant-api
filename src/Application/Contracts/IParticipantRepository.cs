using System.Threading.Tasks;
using Domain.Entities.Participants;

namespace Application.Contracts
{
    public interface IParticipantRepository
    {
        Task<ParticipantDetails> GetParticipantDetailsAsync(string participantId);
        Task<ParticipantDetails> GetParticipantDetailsByEmailAsync(string email);
        Task<ParticipantDemographics> GetParticipantDemographicsAsync(string participantId);
        Task CreateParticipantDetailsAsync(ParticipantDetails entity);
        Task UpdateParticipantDetailsAsync(ParticipantDetails entity);
        Task CreateParticipantDemographicsAsync(ParticipantDemographics entity);
        Task UpdateParticipantDemographicsAsync(ParticipantDemographics entity);

    }
}