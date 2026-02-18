using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Domain.Base;

namespace api.Domain.Users
{
    public class User : BaseEntity
    {
        // --- Identidad ---
        public string Email { get; private set; }= default!; // Se asigna en el constructor
        public string FullName { get; private set; }= default!;

        // --- Seguridad ---
        public string PasswordHash { get; private set; } = default!;

        // --- Estado ---
        public bool IsActive { get; private set; } = true;

        // --- Roles ---
        private readonly HashSet<UserRole> _roles = new();
        public IReadOnlyCollection<UserRole> Roles => _roles;

        // Constructor EF
        private User() { }

        // Constructor de dominio
        public User(string email, string fullName, string passwordHash)
        {
            SetEmail(email);
            SetFullName(fullName);
            SetPasswordHash(passwordHash);
        }

        // --- Comportamiento de dominio ---

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email es obligatorio.", nameof(email));

            // Validación básica (la fuerte va en aplicación)
            Email = email.Trim().ToLowerInvariant();
        }

        public void SetFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("El nombre es obligatorio.", nameof(fullName));

            FullName = fullName.Trim();
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("El hash de contraseña es obligatorio.", nameof(passwordHash));

            PasswordHash = passwordHash;
        }

        public void AddRole(UserRole role)
        {
            _roles.Add(role);
        }

        public void RemoveRole(UserRole role)
        {
            if (_roles.Count == 1)
                throw new InvalidOperationException("El usuario debe tener al menos un rol.");

            _roles.Remove(role);
        }

        public bool HasRole(UserRole role)
            => _roles.Contains(role);

        public void Disable()
        {
            IsActive = false;
        }

        public void Enable()
        {
            IsActive = true;
        }
    }
}