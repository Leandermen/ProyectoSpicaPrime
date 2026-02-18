using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;

namespace api.Application.Services.Queries.GetServiceById;

public sealed class GetServiceByIdHandler
{
    private readonly IServiceRepository _serviceRepository;

    public GetServiceByIdHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<Result<GetServiceByIdResult>> Handle(
        GetServiceByIdQuery query,
        CancellationToken cancellationToken)
    {
        var service = await _serviceRepository
            .GetByIdAsync(query.ServiceId, cancellationToken);

        if (service is null)
            return Result<GetServiceByIdResult>.Fail(
                Errors.Services.NotFound);

        var result = new GetServiceByIdResult(
            service.Id,
            service.Name,
            service.Description,
            service.IsActive
        );

        return Result<GetServiceByIdResult>.Ok(result);
    }
}
