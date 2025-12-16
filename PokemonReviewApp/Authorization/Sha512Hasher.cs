using System.Security.Cryptography;
using System.Text;

namespace PokemonReviewApp.Authorization
{
    public class Sha512Hasher
    {
        // SHA512 + SALT
        public static string HashPassword(string password, string salt)
        {
            using (var sha512 = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password + salt);
                var hash = sha512.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

      
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);
            return Convert.ToBase64String(saltBytes);
        }
    }
}
