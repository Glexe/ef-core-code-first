using cw8.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace cw8
{
    public static class Extensions
    {
        private const int ITERATIONS_COUNT = 5000;
        private const int KEY_BYTES_COUNT = 20;

        public static string StringValue(this Enum enumValue)
        {
            var type = enumValue.GetType();

            FieldInfo fieldInfo = type.GetField(enumValue.ToString());

            var stringValueAttribute = fieldInfo.GetCustomAttribute<StringValueAttribute>();
            return stringValueAttribute?.StringValue;
        }

        public static byte[] Encrypt(this string @string, byte[] salt)
        {
            var encryptedString = new Rfc2898DeriveBytes(@string, salt, ITERATIONS_COUNT);
            return encryptedString.GetBytes(KEY_BYTES_COUNT);
        }

        public static byte[] RenewRefreshToken(this User user, int lifeTimeDays)
        {
            var refreshToken = Guid.NewGuid().ToByteArray();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirationDate = DateTime.Now.AddDays(lifeTimeDays);
            return refreshToken;
        }

        public static SigningCredentials ToSigningCredentials(this string @string)
        {
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(@string));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        public static IApplicationBuilder UseExceptionLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionLoggerMiddleware>();
        }
    }
}
