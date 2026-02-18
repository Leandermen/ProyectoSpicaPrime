using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Application.Common
{
    
/// <summary>
/// Representa el resultado de un caso de uso.
/// Se usa para comunicar éxito o error desde Application hacia el API.
/// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public T? Value { get; }
        public Error? Error { get; }

        private Result(bool isSuccess, T? value, Error? error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        /// <summary>
        /// Crea un resultado exitoso.
        /// </summary>
        public static Result<T> Ok(T value)
            => new(true, value, null);

        /// <summary>
        /// Crea un resultado fallido con un mensaje de error.
        /// </summary>
        public static Result<T> Fail(Error error)
            => new(false, default, error);
    }
}