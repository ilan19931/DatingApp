using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext m_Context;
        public UsersController(DataContext i_Context)
        {
            this.m_Context = i_Context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await m_Context.Users.ToListAsync();
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await m_Context.Users.FindAsync(id);
        }
    }
}