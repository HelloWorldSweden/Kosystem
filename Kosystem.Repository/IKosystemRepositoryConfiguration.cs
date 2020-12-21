using System;

namespace Kosystem.Repository.EF
{
    public interface IKosystemRepositoryConfiguration
        {
            public IKosystemRepositoryConfiguration AddPersonRepositoryMiddleware(
                Func<IServiceProvider, IPersonRepository, IPersonRepository> implementationFactory);
            public IKosystemRepositoryConfiguration AddRoomRepositoryMiddleware(
                Func<IServiceProvider, IRoomRepository, IRoomRepository> implementationFactory);
        }
}
