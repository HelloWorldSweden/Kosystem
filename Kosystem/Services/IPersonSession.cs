using Kosystem.Shared;

namespace Kosystem.Services
{
    public interface IPersonSession
    {
        bool IsRegistered { get; }

        PersonModel RegisterPerson(string name);

        PersonModel? TryGetCurrentPerson();

        bool TryGetCurrentPerson(out PersonModel person);

        PersonModel SetCurrentPerson(string name);
    }
}
