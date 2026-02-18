using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Services;
using api.Domain.Users;

namespace api.Application.Services.Commands.EnableService;

public sealed class EnableServiceHandler
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EnableServiceHandler(
        IServiceRepository serviceRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<EnableServiceResult>> Handle(
        Guid userId,
        EnableServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<EnableServiceResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<EnableServiceResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
            return Result<EnableServiceResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Servicio
        var service = await _serviceRepository.GetByIdAsync(
            command.ServiceId,
            cancellationToken);
        if (service is null)
            return Result<EnableServiceResult>.Fail(Errors.Services.NotFound);

        // 3️⃣ Idempotencia
        if (!service.IsActive)
        {
            service.Enable();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // 4️⃣ Resultado
        return Result<EnableServiceResult>.Ok(
            new EnableServiceResult(
                service.Id,
                service.IsActive
            ));
    }
}
