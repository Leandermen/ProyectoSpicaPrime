using System;

namespace api.Application.Services.Queries.GetServicesByProfessional;

public sealed record GetServicesByProfessionalResult(
    Guid ServiceId,
    string Name,
    string Description,
    bool IsActive
);
