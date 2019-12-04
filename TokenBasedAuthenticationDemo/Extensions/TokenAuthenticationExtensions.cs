
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenBasedAuthenticationDemo.Model;

namespace TokenBasedAuthenticationDemo.Extensions
{
    public static class TokenAuthenticationExtensions
    {
        public static SymmetricSecurityKey ExtractSymmetricSecurityKey(this TokenAuthOptions options)
        {
            var keyBytes = Encoding.UTF8.GetBytes(options.Key);
            return new SymmetricSecurityKey(keyBytes);
        }
    }
}
