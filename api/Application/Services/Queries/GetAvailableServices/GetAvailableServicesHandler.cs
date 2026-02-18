using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Services.Queries.GetAvailableServices;

public sealed class GetAvailableServicesHandler
{
    private readonly IServiceRepository _serviceRepository;

    public GetAvailableServicesHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<IReadOnlyList<GetAvailableServicesResult>>> Handle(
        GetAvailableServicesQuery query,
        CancellationToken cancellationToken)
    {
        var services = await _serviceRepository.GetAvailableAsync(cancellationToken);

        var result = services
            .Select(s => new GetAvailableServicesResult(
                s.Id,
                s.Name,
                s.Description))
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyList<GetAvailableServicesResult>>.Ok(result);
    }
}
