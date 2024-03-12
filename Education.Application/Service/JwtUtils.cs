using Education.Application.Interface;
using Education.Core.Interface;
using Education.Core.Model.Core;
using Education.Core.Model;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Education.Application.Service
{
    public class JwtUtils : IJwtUtils
    {
        private JwtIssuerOptions _jwtIssuerOptions;
        private IUserRepository _userRepository;
        public JwtUtils(IOptions<JwtIssuerOptions> jwtIssuerOptions, IUserRepository userRepository)
        {
            _jwtIssuerOptions = jwtIssuerOptions.Value;
            _userRepository = userRepository;

        }
        public string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserName ?? ""));
            claims.Add(new Claim("UserID", user.UserID.ToString()));
            claims.Add(new Claim("UserName", user.UserName?.ToString() ?? ""));
            claims.Add(new Claim("RoleName", "User"));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtIssuerOptions.SecretKey));

            var tokenRender = new JwtSecurityToken(
            issuer: _jwtIssuerOptions.Issuer,
             audience: _jwtIssuerOptions.Audience,
             expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtIssuerOptions.TokenExpiresAfter)),
             claims: claims,
             signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(tokenRender);
        }

        //public async Task<string> GenerateRefreshToken()
        //{
        //    return await GetUniQueToken();
        //}
        //public async Task<string> GetUniQueToken()
        //{
        //    var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        //    var tokenIsUnique = await _userRepository.GetUserByRefreshToken(token);
        //    if (tokenIsUnique != null)
        //        return await GetUniQueToken();
        //    return token;
        //}
        public string GenerateTokenWithExpiry()
        {
            byte[] tokenBytes = new byte[32]; // Adjust the length of the token as needed
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenBytes);
            }

            string token = Convert.ToBase64String(tokenBytes);
            DateTime expiryDateTime = DateTime.UtcNow.AddMinutes(10);
            string expiryString = expiryDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ");

            return $"{token}:{expiryString}";
        }

        public string GenerateResetLink()
        {
            string baseUrl = "https://example.com/reset-password";
            var tokenWithExpiry = GenerateTokenWithExpiry();
            StringBuilder sb = new StringBuilder(baseUrl);
            sb.Append("?token=");
            sb.Append(Uri.EscapeDataString(tokenWithExpiry));
            return sb.ToString();
        }

        public Task<string> GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}
