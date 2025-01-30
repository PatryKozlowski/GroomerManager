using GroomerManager.Application.Common.Abstraction;
using GroomerManager.Application.Common.Interfaces;
using GroomerManager.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroomerManager.Application.Client;

public abstract class GetClientsCommand
{
    public class Request : IRequest<ClientsResponseDto>
    {
        public Guid SalonId { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }
    }

    public class Handler : BaseCommandHandler, IRequestHandler<Request, ClientsResponseDto>
    {
        private readonly IGroomerManagerDbContext _groomerManagerDb;
        private readonly ICurrentSalonProvider _currentSalonProvider;

        public Handler(IGroomerManagerDbContext groomerManagerDb, ICurrentSalonProvider currentSalonProvider) 
            : base(groomerManagerDb, currentSalonProvider)
        {
            _groomerManagerDb = groomerManagerDb;
            _currentSalonProvider = currentSalonProvider;
        }

        public async Task<ClientsResponseDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var salon = await _currentSalonProvider.GetAuthenticatedSalon(request.SalonId);
            if (salon == null)
            {
                throw new Exception("SalonNotFoundOrUnauthorized");
            }

            var query = _groomerManagerDb.Clients
                .Where(c => c.SalonId == salon.Id);

            query = ApplyFilters(query, request.Search);
            var totalCount = await query.CountAsync(cancellationToken);

            var clientsDto = await ApplyPagination(query, request.Page, request.PageSize)
                .OrderBy(c => c.Created)
                .Select(c => new ClientResponseDto()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                })
                .ToListAsync(cancellationToken);

            return new ClientsResponseDto
            {
                Clients = clientsDto,
                TotalCount = totalCount,
                PageCount = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }

        private IQueryable<Domain.Entities.Client> ApplyFilters(IQueryable<Domain.Entities.Client> query, string? search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(
                    c => EF.Functions.Like(c.FirstName.ToLower(), $"%{searchLower}%") ||
                         EF.Functions.Like(c.LastName.ToLower(), $"%{searchLower}%") ||
                         EF.Functions.Like(c.PhoneNumber.ToLower(), $"%{searchLower}%"));
            }

            return query;
        }

        private IQueryable<Domain.Entities.Client> ApplyPagination(IQueryable<Domain.Entities.Client> query, int page, int pageSize)
        {
            return query.OrderBy(c => c.Created).Skip(page * pageSize).Take(pageSize);
        }
    }
}
