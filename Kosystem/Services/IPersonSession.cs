using System.Diagnostics.CodeAnalysis;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public interface IPersonSession
    {
        bool IsRegistered { get; }

        PersonModel RegisterPerson(string name);

        PersonModel? TryGetCurrentPerson();

        bool TryGetCurrentPerson([NotNullWhen(true)] out PersonModel? person);

        PersonModel SetCurrentPerson(string name);
    }
}
