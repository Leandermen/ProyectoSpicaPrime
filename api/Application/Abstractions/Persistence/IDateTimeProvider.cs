using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Abstractions.Persistence
{
    public interface IDateTimeProvider
    {
        DateTimeOffset UtcNow { get; }
    }
}