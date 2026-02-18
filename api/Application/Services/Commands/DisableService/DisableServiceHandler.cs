using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Services;
using api.Domain.Users;

namespace api.Application.Services.Commands.DisableService;

public sealed class DisableServiceHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DisableServiceHandler(
        IUserRepository userRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DisableServiceResult>> Handle(
        Guid userId,
        DisableServiceCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<DisableServiceResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<DisableServiceResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
            return Result<DisableServiceResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Servicio
        var service = await _serviceRepository.GetByIdAsync(
            command.ServiceId,
            cancellationToken);
        if (service is null)
            return Result<DisableServiceResult>.Fail(Errors.Services.NotFound);

        // 3️⃣ Idempotencia
        if (service.IsActive)
        {
            service.Disable();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // 4️⃣ Resultado
        return Result<DisableServiceResult>.Ok(
            new DisableServiceResult(
                service.Id,
                service.IsActive
            ));
    }
}
