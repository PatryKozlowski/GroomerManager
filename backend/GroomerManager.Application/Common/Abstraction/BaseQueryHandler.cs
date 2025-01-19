using GroomerManager.Application.Common.Interfaces;

namespace GroomerManager.Application.Common.Abstraction;

public abstract class BaseQueryHandler
{
    protected readonly IGroomerManagerDbContext _groomerManagerDb;
    protected readonly ICurrentSalonProvider _currentSalonProvider;
    
    public BaseQueryHandler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider)
    {
        _groomerManagerDb = groomerManagerDb;
        _currentSalonProvider = currentSalonProvider;
    }
}