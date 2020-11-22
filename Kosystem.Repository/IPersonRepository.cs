using Kosystem.Shared;

namespace Kosystem.Repository
{
    public interface IPersonRepository
    {
        PersonModel CreatePerson(NewPersonModel newPerson);

        RemoveResult DeletePerson(int personId);

        PersonModel? UpdatePerson(UpdatePersonModel patch);

        PersonModel? FindPerson(int personId);

        AddResult EnqueuePerson(int personId);

        RemoveResult DequeuePerson(int personId);
    }
}
