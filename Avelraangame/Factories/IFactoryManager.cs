using Persistance_Manager;

namespace Avelraangame.Factories;

public interface IFactoryManager
{
    IDatabaseManager Dbm { get; init; }
    IServiceFactory ServiceFactory { get; init; }
}