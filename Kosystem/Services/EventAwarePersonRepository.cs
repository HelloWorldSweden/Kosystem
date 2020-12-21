using Kosystem.Events;
using Kosystem.Repository;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public class EventAwarePersonRepository : IPersonRepository
    {
        private readonly IPersonRepository _personRepository;
        private readonly IKosystemEvents _kosystemEvents;

        public EventAwarePersonRepository(IPersonRepository personRepository, IKosystemEvents kosystemEvents)
        {
            _personRepository = personRepository;
            _kosystemEvents = kosystemEvents;
        }

        public PersonModel CreatePerson(NewPersonModel newPerson)
        {
            return _personRepository.CreatePerson(newPerson);
        }

        public RemoveResult DeletePerson(long personId)
        {
            return _personRepository.DeletePerson(personId);
        }

        public RemoveResult DequeuePerson(long personId)
        {
            return _personRepository.DequeuePerson(personId);
        }

        public AddResult EnqueuePerson(long personId)
        {
            return _personRepository.EnqueuePerson(personId);
        }

        public PersonModel? FindPerson(long personId)
        {
            return _personRepository.FindPerson(personId);
        }

        public PersonModel? UpdatePerson(UpdatePersonModel patch)
        {
            return _personRepository.UpdatePerson(patch);
        }
    }
}
