using System;
using System.Linq;
using Kosystem.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Kosystem.Repository.EF
{
    internal class EfPersonRepository : IPersonRepository
    {
        private readonly IDbContextFactory<KosystemDbContext> _contextFactory;
        private readonly ILogger<EfPersonRepository> _logger;

        public EfPersonRepository(
            IDbContextFactory<KosystemDbContext> contextFactory,
            ILogger<EfPersonRepository> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public PersonModel CreatePerson(NewPersonModel newPerson)
        {
            var person = new Person
            {
                Name = newPerson.Name
            };

            using var ctx = _contextFactory.CreateDbContext();
            ctx.People.Add(person);
            ctx.SaveChanges();

            return person.ToPersonModel();
        }

        public RemoveResult DeletePerson(int personId)
        {
            try
            {
                using var ctx = _contextFactory.CreateDbContext();
                var person = new Person { Id = personId };
                ctx.Attach(person);
                ctx.Remove(person);

                return ctx.SaveChanges() > 0
                    ? RemoveResult.OK
                    : RemoveResult.AlreadyRemoved;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to remove person by ID '{personId}' from database.");
                return RemoveResult.UnableToRemove;
            }
        }

        public RemoveResult DequeuePerson(int personId)
        {
            try
            {
                using var ctx = _contextFactory.CreateDbContext();
                var person = ctx.People.FirstOrDefault(o => o.Id == personId);

                if (person is null)
                {
                    return RemoveResult.UnableToRemove;
                }

                person.EnqueuedAt = null;

                return ctx.SaveChanges() > 0
                    ? RemoveResult.OK
                    : RemoveResult.AlreadyRemoved;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to dequeue person by ID '{personId}' in database.");
                return RemoveResult.UnableToRemove;
            }
        }

        public AddResult EnqueuePerson(int personId)
        {
            try
            {
                using var ctx = _contextFactory.CreateDbContext();
                var person = ctx.People.FirstOrDefault(o => o.Id == personId);

                if (person is null)
                {
                    return AddResult.UnableToAdd;
                }

                person.EnqueuedAt = DateTime.Now;

                return ctx.SaveChanges() > 0
                    ? AddResult.OK
                    : AddResult.AlreadyAdded;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to enqueue person by ID '{personId}' in database.");
                return AddResult.UnableToAdd;
            }
        }

        public PersonModel? FindPerson(int personId)
        {
            using var ctx = _contextFactory.CreateDbContext();
            return ctx.People.FirstOrDefault(o => o.Id == personId)?.ToPersonModel();
        }

        public PersonModel? UpdatePerson(UpdatePersonModel patch)
        {
            using var ctx = _contextFactory.CreateDbContext();
            var person = ctx.People.FirstOrDefault(o => o.Id == patch.Id);

            if (person is null)
            {
                return null;
            }

            person.Name = patch.Name;

            try
            {
                ctx.SaveChanges();
                return person.ToPersonModel();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to update person by ID '{patch.Id}' in database.");
                return null;
            }
        }
    }
}
