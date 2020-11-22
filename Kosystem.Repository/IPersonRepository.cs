using Kosystem.Shared;

namespace Kosystem.Repository
{
    public interface IPersonRepository
    {
        PersonModel CreatePerson(NewPersonModel newPerson);

        RemoveResult DeletePerson(long personId);

        PersonModel? UpdatePerson(UpdatePersonModel patch);

        PersonModel? FindPerson(long personId);

        AddResult EnqueuePerson(long personId);

        RemoveResult DequeuePerson(long personId);
    }
}
