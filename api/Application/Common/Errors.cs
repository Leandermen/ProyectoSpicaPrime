using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Common;

public static class Errors
{
    public static class Common
    {
        public static readonly Error Unauthorized =
            new("Common.Unauthorized", "No estás autenticado.",ErrorType.Unauthorized);

        public static readonly Error Forbidden =
            new("Common.Forbidden", "No tienes permisos para esta acción.",ErrorType.Unauthorized);

        public static readonly Error InvalidOperation =
            new("Common.InvalidOperation", "La operación no es válida en el estado actual.",ErrorType.Validation);
    }

    

    public static class Users
    {
        public static readonly Error NotFound =
            new("Users.NotFound", "El usuario no existe.",ErrorType.NotFound);

        public static readonly Error AlreadyExists =
            new("Users.AlreadyExists", "El usuario ya existe registrado con ese E-mail.",ErrorType.Conflict);
        public static readonly Error Inactive =
            new("Users.Inactive", "El usuario no está activo.",ErrorType.Unauthorized);
    }

    public static class Services
    {
        public static readonly Error NotFound =
            new("Services.NotFound", "El servicio no existe.",ErrorType.NotFound);
        public static readonly Error AlreadyExists =
            new("Services.AlreadyExists", "El servicio ya existe.",ErrorType.Conflict);
        public static readonly Error NotAvailable =
            new("Services.NotAvailable", "El servicio no está disponible.",ErrorType.Unauthorized);
        public static readonly Error BlankName =
            new("Services.BlankName", "El nombre del servicio no puede estar en blanco.",ErrorType.Validation);
        public static readonly Error BlankDescription =
            new("Services.BlankDescription", "La descripción del servicio no puede estar en blanco.",ErrorType.Validation);
        public static readonly Error InvalidEstimatedDuration =
            new("Services.InvalidEstimatedDuration", "La duración estimada debe ser mayor que 0.",ErrorType.Validation);
    }

    public static class Agreements
    {
        public static readonly Error NotFound =
            new("Agreements.NotFound", "El acuerdo no existe.",ErrorType.NotFound);

        public static readonly Error InvalidState =
            new("Agreements.InvalidState", "El acuerdo no permite esta acción.",ErrorType.Validation);
    }

    public static class Works
    {
        public static readonly Error NotFound =
            new("Works.NotFound", "El trabajo no existe.",ErrorType.NotFound);

        public static readonly Error InvalidState =
            new("Works.InvalidState", "El estado del trabajo no permite esta acción.",ErrorType.Validation);
        
        public static readonly Error EmptyReason =
            new("Works.EmptyReason", "El motivo de suspensión no puede estar vacío.",ErrorType.Validation);
    }

    public static class WorkRequests
    {
        public static readonly Error NotFound =
            new("WorkRequests.NotFound", "La solicitud de trabajo no existe.",ErrorType.NotFound);

        public static readonly Error InvalidState =
            new("WorkRequests.InvalidState", "El estado de la solicitud no permite esta acción.",ErrorType.Validation);
    }
}
