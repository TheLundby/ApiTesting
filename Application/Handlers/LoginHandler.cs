using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Handlers
{
    public class LoginHandler
    {
        private readonly IConfiguration _config;

        public LoginHandler(IConfiguration config)
        {
            _config = config;
        }

        public string? Handle(Credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Username) || string.IsNullOrEmpty(credentials.Password))
            {
                throw new Exception("missing username or password");
            }

            if (credentials.Username != _config["admin_username"] || credentials.Password != _config["admin_password"])
            {
                return null;
            }

            // 🔑 2. Create claims (data inside JWT)
            Claim[] claims = {
                new Claim(ClaimTypes.Name, credentials.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            // 🔐 3. Create signing key
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]!)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 🧾 4. Create token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            // 📦 5. Return token as string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
