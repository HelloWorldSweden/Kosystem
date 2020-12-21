using System;

namespace Kosystem.Repository
{
    public abstract class KosystemRepositoryConfiguration
    {
        protected Func<IServiceProvider, IPersonRepository, IPersonRepository>? PersonRepoFactory { get; set; }
        protected Func<IServiceProvider, IRoomRepository, IRoomRepository>? RoomRepoFactory { get; set; }

        protected abstract IPersonRepository CreateInitialPersonRepository(IServiceProvider provider);
        protected abstract IRoomRepository CreateInitialRoomRepository(IServiceProvider provider);
        
        public KosystemRepositoryConfiguration AddPersonRepositoryMiddleware(Func<IServiceProvider, IPersonRepository, IPersonRepository> implementationFactory)
        {
            PersonRepoFactory = InternalAddMiddleware(PersonRepoFactory, implementationFactory);
            return this;
        }

        public KosystemRepositoryConfiguration AddRoomRepositoryMiddleware(Func<IServiceProvider, IRoomRepository, IRoomRepository> implementationFactory)
        {
            RoomRepoFactory = InternalAddMiddleware(RoomRepoFactory, implementationFactory);
            return this;
        }

        public IPersonRepository CreatePersonFactory(IServiceProvider provider)
        {
            return CreateRepoFactory(provider, CreateInitialPersonRepository, PersonRepoFactory);
        }

        public IRoomRepository CreateRoomFactory(IServiceProvider provider)
        {
            return CreateRepoFactory(provider, CreateInitialRoomRepository, RoomRepoFactory);
        }

        public static T CreateRepoFactory<T>(IServiceProvider provider, Func<IServiceProvider, T> initialFactory, Func<IServiceProvider, T, T>? middlewares)
        {
            var repo = initialFactory(provider);

            if (middlewares is not null)
            {
                return middlewares(provider, repo);
            }

            return repo;
        }

        public static Func<IServiceProvider, T, T> InternalAddMiddleware<T>(Func<IServiceProvider, T, T>? oldFactory, Func<IServiceProvider, T, T> newFactory)
        {
            if (oldFactory is null)
            {
                return newFactory;
            }
            else
            {
                return (provider, repo) => newFactory(provider, oldFactory(provider, repo));
            }
        }
    }
}
