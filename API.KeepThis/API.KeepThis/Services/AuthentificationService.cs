﻿using API.KeepThis.Helpers;
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


        public async Task<LoginUserDto> AuthenticateUserAsync(LoginDto request)
        {
            var user = await _UsersRepository.GetByEmailAsync(request.Email);

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Le compte a été désactivé");
            }

            if (user == null || !_passwordHasher.VerifyPassword(user.PasswordUser, request.Password))
            {
                throw new UnauthorizedAccessException("email ou mot de passe incorrect");
            }

            if (string.IsNullOrEmpty(user.CertifiedEmailUser))
            {
                // Email is not certified
                throw new UnauthorizedAccessException("email non certifié. Echec de connexion.");

            }

            if (user.LockedOutEnd.HasValue && user.LockedOutEnd.Value > DateTime.UtcNow)
            {
                // User is currently locked out
                throw new UnauthorizedAccessException("Nombre d'erreur de connexion dépassé. Veuillez réessayer plus tard.");
            }

            if (!_passwordHasher.VerifyPassword(user.PasswordUser, request.Password))
            {
                // Increment failed login attempts
                user.FailedLoginAttemps++;

                if (user.FailedLoginAttemps >= 3)
                {
                    // Set lockout end time
                    user.LockedOutEnd = DateTime.UtcNow.AddMinutes(5);
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
                TimestampToken = DateTime.UtcNow
            };

            await _tokenRepository.AddTokenAsync(userToken);         

            return new LoginUserDto { User = user, Token=token };
        }
    }
}