using API.KeepThis.Helpers;
using API.KeepThis.Model;
using API.KeepThis.Model.DTO;
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
        private readonly IPasswordSecurity _passwordSecurity;
        private readonly IUsersRepository _UsersRepository;
        private readonly IAuthTokenRepository _tokenRepository;
        private readonly string _secretKey;

        public AuthentificationService(IJwtSettings jwtSettings, IPasswordSecurity passwordSecurity, IUsersRepository UsersRepository, IAuthTokenRepository authTokenRepository)
        {
            _secretKey = jwtSettings.SecretKey;
            _passwordSecurity = passwordSecurity;
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


        public async Task<LoginUserDto> AuthenticateUserAsync(LoginDto request)
        {
            var user = await _UsersRepository.GetByEmailAsync(request.Email);

            if (user == null || !_passwordSecurity.VerifyPassword(user.PasswordUser, request.Password, user.SaltUser))
            {
                throw new UnauthorizedAccessException("email ou mot de passe incorrect");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Le compte a été désactivé");
            }

            if (string.IsNullOrEmpty(user.CertifiedEmailUser))
            { 
                // Email is not certified
                throw new UnauthorizedAccessException("Echec de connexion.Veuillez certifier votre email");

            }

            if (user.LockedOutEnd.HasValue && user.LockedOutEnd.Value > DateTime.UtcNow)
            {
                // User is currently locked out
                throw new UnauthorizedAccessException("Nombre d'erreur de connexion dépassé. Veuillez réessayer plus tard.");
            }

            if (!_passwordSecurity.VerifyPassword(user.PasswordUser, request.Password,user.SaltUser))
            {
                // Increment failed login attempts
                user.FailedLoginAttemps++;

                if (user.FailedLoginAttemps >= 3)
                {
                    // Set lockout end time
                    user.LockedOutEnd = DateTime.Now.AddMinutes(5);
                }

                await _UsersRepository.UpdateAsync(user);

                throw new UnauthorizedAccessException("email ou mot de passe incorrect");
            }

            // Reset failed login attempts on successful login
            user.FailedLoginAttemps = 0;
            user.LockedOutEnd = null;
            await _UsersRepository.UpdateAsync(user);

            var token = GenerateJwtToken(user);

            // Store the token in the database
            var userToken = new AuthentificationToken
            {
                IdUser = user.IdUser,
                Token = token,
                TimestampToken = DateTime.Now
            };

            await _tokenRepository.AddTokenAsync(userToken);         

            return new LoginUserDto { User = user, Token=token };
        }

        public string GenerateEmailVerificationToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddHours(1), // Token expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                // Set a specific audience and issuer for verification tokens if needed
                Audience = "EmailVerification",
                Issuer = "KeepThisAPI"
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> VerifyEmailAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = "KeepThisAPI",
                    ValidAudience = "EmailVerification",
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;

                if (email == null)
                {
                    return false; // Invalid token
                }

                var user = await _UsersRepository.GetByEmailAsync(email);

                if (user == null)
                {
                    return false; // User not found or already verified
                }

                user.CertifiedEmailUser = email;
 

                await _UsersRepository.UpdateAsync(user);

                return true; // Email verified successfully
            }
            catch
            {
                return false; // Token validation failed
            }
        }
    }
}
