using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Abstractions.Security
{
    public interface IPasswordHasher
    {
        string Hash(string plainPassword);
    }
}