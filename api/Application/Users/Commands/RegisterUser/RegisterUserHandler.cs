using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Application.Abstractions.Persistence;
using api.Application.Abstractions.Persistence.Repositories;
using api.Application.Abstractions.Security;
using api.Application.Common;
using api.Domain.Users;

namespace api.Application.Users.Commands.RegisterUser
{
    public sealed class RegisterUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand command, CancellationToken cancellationToken = default)
        {
            // 1️⃣ Regla: email único
            var existingUser = await _userRepository
                .GetByEmailAsync(command.Email, cancellationToken);

            if (existingUser is not null)
                return Result<RegisterUserResult>.Fail(Errors.Users.AlreadyExists);

            // 2️⃣ Hash de contraseña
            var passwordHash = _passwordHasher.Hash(command.Password);

            // 3️⃣ Crear entidad de dominio
            var user = new User(
                command.Email,
                command.FullName,
                passwordHash
            );

            // Agregar Rol por defecto
            user.AddRole(UserRole.Client);

            // 4️⃣ Persistir
            await _userRepository.AddAsync(user, cancellationToken);

            // 5️⃣ Confirmar transacción
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 6️⃣ Resultado del caso de uso
            return Result<RegisterUserResult>.Ok(
                new RegisterUserResult(
                    user.Id,
                    user.Email
                ));
        }
    }
}