using System;
using System.Diagnostics.CodeAnalysis;
using Kosystem.Repository;
using Kosystem.Shared;

namespace Kosystem.Services
{
    public class PersonSession : IDisposable, IPersonSession
    {
        private readonly EventAwareRepositories _eventAwareRepos;
        private readonly IPersonRepository _personRepository;
        private bool _disposedValue;
        private long? _registeredPersonId;

        public bool IsRegistered => _registeredPersonId.HasValue;

        public PersonSession(
            IPersonRepository personRepository,
            EventAwareRepositories eventAwareRepos)
        {
            _personRepository = personRepository;
            _eventAwareRepos = eventAwareRepos;
        }

        public PersonModel RegisterPerson(string name)
        {
            if (_registeredPersonId.HasValue)
            {
                throw new InvalidOperationException("A user is already registered for this session.");
            }

            var person = _personRepository.CreatePerson(new NewPersonModel(name));
            _registeredPersonId = person.Id;
            return person;
        }

        public PersonModel? TryGetCurrentPerson()
        {
            if (!_registeredPersonId.HasValue)
            {
                return null;
            }

            return _personRepository.FindPerson(_registeredPersonId.Value);
        }

        public bool TryGetCurrentPerson([NotNullWhen(true)] out PersonModel? person)
        {
            if (!_registeredPersonId.HasValue)
            {
                person = default;
                return false;
            }

            var possiblyPerson = _personRepository.FindPerson(_registeredPersonId.Value);
            if (possiblyPerson is null)
            {
                person = default;
                return false;
            }

            person = possiblyPerson;
            return true;
        }

        private void UnregisterUser()
        {
            if (_registeredPersonId.HasValue)
            {
                if (TryGetCurrentPerson(out var person))
                {
                    _eventAwareRepos.DeletePerson(person);
                }

                _registeredPersonId = null;
            }
        }

        public PersonModel SetCurrentPerson(string name)
        {
            if (TryGetCurrentPerson(out var person))
            {
                if (!person.Name.Equals(name, StringComparison.Ordinal))
                {
                    var updatedPerson = _personRepository.UpdatePerson(new UpdatePersonModel(person.Id, name));
                    if (updatedPerson is not null)
                    {
                        return updatedPerson;
                    }
                    else
                    {
                        throw new InvalidOperationException("Failed to update person. Maybe it was removed just as the update was being applied?");
                    }
                }

                return person;
            }

            return RegisterPerson(name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    UnregisterUser();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
