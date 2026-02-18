using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Common;
using api.Domain.Agreements;
using api.Domain.Services;
using api.Domain.Users;

namespace api.Application.Agreements.Commands.CreateAgreementTemplate
{
    public sealed class CreateAgreementTemplateHandler
    {
        private readonly IAgreementTemplateRepository _agreementTemplateRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAgreementTemplateHandler(
            IAgreementTemplateRepository agreementTemplateRepository,
            IServiceRepository serviceRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _agreementTemplateRepository = agreementTemplateRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateAgreementTemplateResult>> Handle(
            Guid userId,
            CreateAgreementTemplateCommand command,
            CancellationToken cancellationToken = default)
        {
            // 1️⃣ Usuario
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user is null)
                return Result<CreateAgreementTemplateResult>.Fail(Errors.Users.NotFound);

            if (!user.IsActive)
                return Result<CreateAgreementTemplateResult>.Fail(Errors.Users.Inactive);

            if (!user.HasRole(UserRole.Professional) && !user.HasRole(UserRole.Admin))
                return Result<CreateAgreementTemplateResult>.Fail(Errors.Common.Forbidden);

            // 2️⃣ Servicio
            var service = await _serviceRepository.GetByIdAsync(
                command.ServiceId,
                cancellationToken);
            if (service is null)
                return Result<CreateAgreementTemplateResult>.Fail(Errors.Services.NotFound);
                
            if (!service.IsActive)
                return Result<CreateAgreementTemplateResult>.Fail(Errors.Services.NotAvailable);

            // 3️⃣ Validaciones de input
            if (string.IsNullOrWhiteSpace(command.Content))
                return Result<CreateAgreementTemplateResult>.Fail(Errors.AgreementTemplates.NullContent);

            // 4️⃣ Versión (Application decide)
            var lastVersion =
                await _agreementTemplateRepository.GetLastVersionByServiceIdAsync(
                    command.ServiceId,
                    cancellationToken);

            var nextVersion = lastVersion + 1;

            // 5️⃣ Crear AgreementTemplate (Draft)
            var template = new AgreementTemplate(
                command.ServiceId,
                nextVersion,
                command.Content.Trim()
            );

            // 6️⃣ Persistir
            await _agreementTemplateRepository.AddAsync(
                template,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7️⃣ Resultado
            return Result<CreateAgreementTemplateResult>.Ok(
                new CreateAgreementTemplateResult(
                    template.Id,
                    template.ServiceId,
                    template.Version,
                    template.Status
                )
            );
        }
    }
}