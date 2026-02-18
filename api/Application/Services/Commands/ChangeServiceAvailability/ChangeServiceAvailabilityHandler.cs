using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Services;
using api.Domain.Users;

namespace api.Application.Services.Commands.ChangeServiceAvailability;

public sealed class ChangeServiceAvailabilityHandler
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeServiceAvailabilityHandler(
        IServiceRepository serviceRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ChangeServiceAvailabilityResult>> Handle(
        Guid userId,
        ChangeServiceAvailabilityCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<ChangeServiceAvailabilityResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<ChangeServiceAvailabilityResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
            return Result<ChangeServiceAvailabilityResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Buscar servicio
        var service = await _serviceRepository.GetByIdAsync(
            command.ServiceId,
            cancellationToken);
            if (service is null)
                return Result<ChangeServiceAvailabilityResult>.Fail(Errors.Services.NotFound);

        if (!service.IsActive)
            return Result<ChangeServiceAvailabilityResult>.Fail(Errors.Common.InvalidOperation);

         // 3️⃣ Idempotencia
        if (service.IsAvailable != command.IsAvailable)
        {
            service.SetAvailability(command.IsAvailable);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // 4️⃣ Resultado
        return Result<ChangeServiceAvailabilityResult>.Ok(
            new ChangeServiceAvailabilityResult(
                service.Id,
                service.IsAvailable
            ));
    }
}
