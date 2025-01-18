using GroomerManager.Application.Common.Interfaces;

namespace GroomerManager.Application.Common.Abstraction;

public abstract class BaseQueryHandler
{
    protected readonly IGroomerManagerDbContext _groomerManagerDb;

    public BaseQueryHandler(IGroomerManagerDbContext groomerManagerDb)
    {
        _groomerManagerDb = groomerManagerDb;
    }
}