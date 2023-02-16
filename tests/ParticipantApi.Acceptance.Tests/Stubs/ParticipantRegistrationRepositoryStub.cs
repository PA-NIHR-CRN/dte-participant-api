using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Contracts;
using Domain.Entities.ParticipantRegistrations;

namespace ParticipantApi.Acceptance.Tests.Stubs
{
    public class ParticipantRegistrationRepositoryStub : IParticipantRegistrationRepository
    {
        private readonly ConcurrentBag<ParticipantRegistration> _participantRegistrations;

        public ParticipantRegistrationRepositoryStub()
        {
            _participantRegistrations = new ConcurrentBag<ParticipantRegistration>();
        }

        private static bool StringEquals(string stringA, string stringB)
        {
            return string.Equals(stringA, stringB, StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteAsync(long studyId, string siteId)
        {
            var items = _participantRegistrations.Where(x => x.StudyId == studyId && StringEquals(siteId, x.SiteId));
            
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudyAsync(long studyId, string participantId)
        {
            var items = _participantRegistrations.Where(x => x.StudyId == studyId && StringEquals(participantId, x.ParticipantId));
            
            return await Task.FromResult(items);
        }

        public async Task<ParticipantRegistration> GetParticipantByStudySiteAsync(long studyId, string siteId, string participantId)
        {
            var item = _participantRegistrations.FirstOrDefault(x => x.StudyId == studyId && StringEquals(siteId, x.SiteId) && StringEquals(participantId, x.ParticipantId));

            return await Task.FromResult(item);
        }

        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantsByStudySiteStatusAsync(long studyId, string siteId, ParticipantRegistrationStatus participantRegistrationStatus)
        {
            var items = _participantRegistrations.Where(x => x.StudyId == studyId && StringEquals(siteId, x.SiteId) && x.ParticipantRegistrationStatus == participantRegistrationStatus);
            
            return await Task.FromResult(items);
        }

        public async Task<IEnumerable<ParticipantRegistration>> GetParticipantRegistrationsStatusByStudy(long studyId, ParticipantRegistrationStatus participantRegistrationStatus)
        {
            var items = _participantRegistrations.Where(x => x.StudyId == studyId && x.ParticipantRegistrationStatus == participantRegistrationStatus);
            
            return await Task.FromResult(items);
        }

        public async Task CreateParticipantRegistrationAsync(ParticipantRegistration entity)
        {
            _participantRegistrations.Add(entity);

            await Task.CompletedTask;
        }

        public async Task SaveParticipantRegistrationAsync(ParticipantRegistration entity)
        {
            await Task.CompletedTask;
        }
    }
}