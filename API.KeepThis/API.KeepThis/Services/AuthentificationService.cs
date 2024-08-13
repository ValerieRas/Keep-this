using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Repositories;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.KeepThis.Services
{
    public class AuthentificationService : IAuthentificationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUsersRepository _UsersRepository;
        private readonly IAuthTokenRepository _tokenRepository;
        private readonly string _secretKey;

        public AuthentificationService(IJwtSettings jwtSettings, IPasswordHasher passwordHasher, IUsersRepository UsersRepository, IAuthTokenRepository authTokenRepository)
        {
            _secretKey = jwtSettings.SecretKey;
            _passwordHasher = passwordHasher;
            _UsersRepository = UsersRepository;
            _tokenRepository = authTokenRepository;
        }

        public string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.CertifiedEmailUser),
                // Add any additional claims here
            }),

                Expires = DateTime.UtcNow.AddMonths(2), // Set the token expiration time

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public async Task<User> AuthenticateUserAsync(LoginRequest request)
        {
            var user = await _UsersRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordUser, request.Password))
            {
                throw new UnauthorizedAccessException("email ou mot de passe incorrect");
            }

            if (string.IsNullOrEmpty(user.CertifiedEmailUser))
            {
                // Email is not certified
                throw new UnauthorizedAccessException("email non certifié. Echec de connexion.");

            }

            var token = GenerateJwtToken(user);

            // Store the token in the database
            var userToken = new AuthentificationToken
            {
                IdUser = user.CertifiedEmailUser, // or user.Id if you have a unique user identifier
                Token = token,
                TimestampToken = DateTime.UtcNow
            };

            await _tokenRepository.AddTokenAsync(userToken);

            return new User { CertifiedEmailUser = user.CertifiedEmailUser };
        }
    }
}
