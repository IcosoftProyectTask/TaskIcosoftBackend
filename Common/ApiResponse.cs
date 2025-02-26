using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIcosoftBackend.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public required string Message { get; set; }
        public T? Data { get; set; }
        public static ApiResponse<T> Ok(T? data = default, string message = "Operación exitosa")
        {
            return new ApiResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Error(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));
            }

            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        public static ApiResponse<T> Error(string message, T? data)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("El mensaje no puede ser nulo o vacío.", nameof(message));
            }

            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Data = data
            };
        }
    }
}