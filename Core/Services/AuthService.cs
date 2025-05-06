﻿using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ServicesAbstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager , IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnAuthorizedException();
            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!flag) throw new UnAuthorizedException();
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateGwtTokenAsync(user),

            };
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description);
                throw new ValidationExceptions(errors);
            }
            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateGwtTokenAsync(user),

            };
        }


        private async Task<string> GenerateGwtTokenAsync (AppUser user)
        {
            var JwtOptions = options.Value;

            var AuthClaim = new List<Claim>() {
                new Claim (ClaimTypes.Name , user.UserName),
                new Claim (ClaimTypes.Email,user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles) { 
            AuthClaim.Add(new Claim (ClaimTypes.Role, role));   
            }
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer : JwtOptions.Issuer,
                audience : JwtOptions.Audience,
                claims : AuthClaim,
                expires : DateTime.UtcNow.AddDays(JwtOptions.DurationInDays),
                signingCredentials : new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }








    }
}