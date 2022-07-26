using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;

        }

        [HttpPost("register")]
        // public async Task<ActionResult<AppUser>> Register(string username, string password) // gets the parameters from the url or qurey string
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) // get the parameters from the body
        {
            //returns badrequest if user already exists
            if (await UserExists(registerDto.UserName)) return BadRequest("UserName is taken");

            //"using" will call the dispose method once done with our work inside HMAC -> HashAlgorithm -> IDisposable
            using var hmac = new HMACSHA512();  //algorithm to create hashing

            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                //Encoding.UTF8.GetBytes(password) converts the string to byte array
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            //Add the user to Database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                userName = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            #region username check
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null) return Unauthorized("Invalid username");
            #endregion

            #region password check
            //get the password hash using the password salt
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }
            #endregion
            return new UserDto
            {
                userName = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }

        //protected class so that it can be used only inside this namespace to not allow duplicate usernames
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


    }
}