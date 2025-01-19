using GroomerManager.Application.Common.Interfaces;

namespace GroomerManager.Application.Common.Abstraction;

public abstract class BaseCommandHandler
{
    protected readonly IGroomerManagerDbContext _groomerManagerDb;
    protected readonly ICurrentSalonProvider _currentSalonProvider;

    public BaseCommandHandler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider)
    {
        _groomerManagerDb = groomerManagerDb;
        _currentSalonProvider = currentSalonProvider;
    }
}