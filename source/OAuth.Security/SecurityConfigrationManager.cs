using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using OAuth.Security.Interface;
using System;
using System.Security.Cryptography;

namespace OAuth.Security
{
    public static class SecurityConfigrationManager
    {
        /// <summary>
        /// This settings must be assign at application Start
        /// </summary>
        public static ISecuritySettings SecuritySettings { internal get; set; }

        internal static string ComputeHash(string password, byte[] salt)
        {
            return Convert.ToBase64String(
              KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
              )
            );
        }

        internal static byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[128 / 8];

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return salt;
        }
    }
}
