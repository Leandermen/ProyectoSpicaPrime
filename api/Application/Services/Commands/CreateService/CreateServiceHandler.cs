using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Services;
using api.Domain.Users;

namespace api.Application.Services.Commands.CreateService;

public sealed class CreateServiceHandler
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceHandler(
        IServiceRepository serviceRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateServiceResult>> Handle(
        Guid userId,
        CreateServiceCommand command,
        CancellationToken cancellationToken = default)
{
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<CreateServiceResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<CreateServiceResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Professional))
            return Result<CreateServiceResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Validaciones de input
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<CreateServiceResult>.Fail(Errors.Services.BlankName);

        if (string.IsNullOrWhiteSpace(command.Description))
            return Result<CreateServiceResult>.Fail(Errors.Services.BlankDescription);

        if (command.EstimatedDurationDays <= 0)
            return Result<CreateServiceResult>.Fail(Errors.Services.InvalidEstimatedDuration);

        // 3️⃣ Unicidad
        var exists = await _serviceRepository.ExistsByNameAsync(
            command.Name.Trim(),
            cancellationToken);

        if (exists)
            return Result<CreateServiceResult>.Fail(Errors.Services.AlreadyExists);

        // 4️⃣ Crear entidad
        var service = new Service(
            command.Name.Trim(),
            command.Description.Trim(),
            command.EstimatedDurationDays
        );

        // 5️⃣ Persistir
        await _serviceRepository.AddAsync(service, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return Result<CreateServiceResult>.Ok(
            new CreateServiceResult(
                service.Id,
                service.Name,
                service.IsAvailable,
                service.IsActive
            ));
    }
}
