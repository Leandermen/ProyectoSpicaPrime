using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Agreements;
using api.Domain.Users;

namespace api.Application.Agreements.Commands.ActivateAgreementTemplate;

public sealed class ActivateAgreementTemplateHandler
{
    private readonly IAgreementTemplateRepository _agreementTemplateRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateAgreementTemplateHandler(
        IAgreementTemplateRepository agreementTemplateRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _agreementTemplateRepository = agreementTemplateRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ActivateAgreementTemplateResult>> Handle(
        Guid userId,
        ActivateAgreementTemplateCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1️⃣ Usuario
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result<ActivateAgreementTemplateResult>.Fail(Errors.Users.NotFound);

        if (!user.IsActive)
            return Result<ActivateAgreementTemplateResult>.Fail(Errors.Users.Inactive);

        if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
            return Result<ActivateAgreementTemplateResult>.Fail(Errors.Common.Forbidden);

        // 2️⃣ Template
        var template = await _agreementTemplateRepository.GetByIdAsync(
            command.AgreementTemplateId,
            cancellationToken);
        if (template is null)
            return Result<ActivateAgreementTemplateResult>.Fail(Errors.AgreementTemplates.NotFound);

        if (template.Status != AgreementTemplateStatus.Draft)
            return Result<ActivateAgreementTemplateResult>.Fail(Errors.AgreementTemplates.InvalidState);

        // 3️⃣ Archivar template activo existente (si hay)
        var activeTemplate =
            await _agreementTemplateRepository.GetActiveByServiceIdAsync(
                template.ServiceId,
                cancellationToken);

        if (activeTemplate is not null)
        {
            activeTemplate.Archive();
            await _agreementTemplateRepository.UpdateAsync(
                activeTemplate,
                cancellationToken);
        }

        // 4️⃣ Activar template
        template.Activate();
        await _agreementTemplateRepository.UpdateAsync(
            template,
            cancellationToken);

        // 5️⃣ Persistir
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 6️⃣ Resultado
        return Result<ActivateAgreementTemplateResult>.Ok(
            new ActivateAgreementTemplateResult(
                template.Id,
                template.Status
            ));
    }
}
