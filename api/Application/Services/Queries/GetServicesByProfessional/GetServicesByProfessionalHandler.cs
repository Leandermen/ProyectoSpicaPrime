using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Services.Queries.GetServicesByProfessional;

public sealed class GetServicesByProfessionalHandler
{
    private readonly IServiceRepository _serviceRepository;

    public GetServicesByProfessionalHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<IReadOnlyList<GetServicesByProfessionalResult>>> Handle(
        GetServicesByProfessionalQuery query,
        CancellationToken cancellationToken)
    {
        var services = await _serviceRepository
            .GetByProfessionalIdAsync(query.ProfessionalId, cancellationToken);

        if (services.Count == 0)
        {
            return Result<IReadOnlyList<GetServicesByProfessionalResult>>
                .Fail(Errors.Services.NotFound);
        }

        var result = services
            .Select(s => new GetServicesByProfessionalResult(
                s.Id,
                s.Name,
                s.Description,
                s.IsActive))
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyList<GetServicesByProfessionalResult>>.Ok(result);
    }
}
