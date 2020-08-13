using Kosystem.Shared;

namespace Kosystem.Services
{
    public interface IPersonRepository
    {
        PersonModel CreatePerson(NewPersonModel patch);

        bool DeletePerson(int personId);

        PersonModel? UpdatePerson(UpdatePersonModel patch);

        PersonModel? FindPerson(int personId);

        bool EnqueuePerson(int personId);

        bool DequeuePerson(int personId);
    }
}
