using GroomerManager.Application.Common.Interfaces;

namespace GroomerManager.Application.Common.Abstraction;

public abstract class BaseCommandHandler
{
    protected readonly IGroomerManagerDbContext _groomerManagerDb;

    public BaseCommandHandler(IGroomerManagerDbContext groomerManagerDb)
    {
        _groomerManagerDb = groomerManagerDb;
    }
}