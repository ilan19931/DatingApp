using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext m_Context;
        private readonly ITokenService m_TokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.m_Context = context;
            this.m_TokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            // check if username is taken
            if(await UserExists(registerDTO.Username)) return BadRequest("Username is allready taken!");

            AppUser newUser = null;

            using (HMACSHA512 hmac = new HMACSHA512())
            {

                newUser = new AppUser{
                    UserName = registerDTO.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hmac.Key
                };

                m_Context.Users.Add(newUser);
            }

            await m_Context.SaveChangesAsync();

            return new UserDTO{
                Username = newUser.UserName,
                Token = m_TokenService.CreateToken(newUser)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await m_Context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            AppUser user = await m_Context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == loginDTO.Username);
        
            if(user == null)
                return Unauthorized("wrong details!");

            HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("wrong details!");
                }
            }

            return new UserDTO{
                Username = user.UserName,
                Token = m_TokenService.CreateToken(user)
            };
        }
    }
}