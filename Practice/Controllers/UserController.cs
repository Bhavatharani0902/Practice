using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Practice.DTOs;
using Practice.Entities;
using Practice.Service;
using System;
using log4net;
using Microsoft.IdentityModel.Tokens;
using Practice.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILog _logger;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IMapper mapper, ILog logger, IConfiguration configuration)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration;
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        public IActionResult AddUser(UserDTO userDto)
        {
            try
            {
                User user = _mapper.Map<User>(userDto);
                _userService.CreateUser(user);
                _logger.Info($"User registered successfully. User ID: {user.UserId}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error registering user: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Validate")]
        [AllowAnonymous]
        public IActionResult Validate(Login login)
        {
            try
            {
                User user = _userService.ValidateUser(login.Email, login.Password);
                AuthResponse authResponse = new AuthResponse();
                if (user != null)
                {
                    authResponse.UserID = user.UserId;
                    authResponse.UserName = user.Username;
                    authResponse.Role = user.Role;
                    authResponse.Token = GenerateToken(user);
                }
                _logger.Info($"User validated successfully. User ID: {user?.UserId}");
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error validating user: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        private string GenerateToken(User? user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature
            );

            var claims = new List<Claim>
            {
                
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var identity = new ClaimsIdentity(claims);
            var expires = DateTime.UtcNow.AddMinutes(10);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}

