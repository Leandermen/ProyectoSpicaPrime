using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Agreements;
using api.Domain.Users;

namespace api.Application.Services.Commands.RequestService;

public sealed class RequestServiceHandler
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IAgreementTemplateRepository _agreementTemplateRepository;
    private readonly IAgreementRepository _agreementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestServiceHandler(
        IServiceRepository serviceRepository,
        IUserRepository userRepository,
        IAgreementTemplateRepository agreementTemplateRepository,
        IAgreementRepository agreementRepository,
        IUnitOfWork unitOfWork)
    {
        _serviceRepository = serviceRepository;
        _userRepository = userRepository;
        _agreementTemplateRepository = agreementTemplateRepository;
        _agreementRepository = agreementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RequestServiceResult>> Handle(
        Guid clientId,
        RequestServiceCommand command,
        CancellationToken cancellationToken)
    {
        // ⚠️ Por ahora simulamos usuario
        var client = await _userRepository.GetByIdAsync(
            clientId, cancellationToken);

        if (client is null)
            return Result<RequestServiceResult>.Fail(
                Errors.Users.NotFound);

        if (!client.IsActive || !client.HasRole(UserRole.Client))
            return Result<RequestServiceResult>.Fail(
                Errors.Common.Forbidden);

        var service = await _serviceRepository
            .GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result<RequestServiceResult>.Fail(
                Errors.Services.NotFound);

        if (!service.IsActive || !service.IsAvailable)
            return Result<RequestServiceResult>.Fail(
                Errors.Services.NotAvailable);

        var template =
            await _agreementTemplateRepository
                .GetActiveByServiceIdAsync(service.Id, cancellationToken);

        if (template is null)
            return Result<RequestServiceResult>.Fail(
                Errors.Agreements.NotFound);

        var agreement = new Agreement(
            service.Id,
            template.Id,
            client.Id,
            template.Content);

        await _agreementRepository.AddAsync(agreement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<RequestServiceResult>.Ok(
            new RequestServiceResult(
                agreement.Id,
                service.Id,
                client.Id,
                agreement.Status));
    }
}
