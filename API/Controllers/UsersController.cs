using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using API.Entities;
using System.Linq;
using API.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    public class UsersController :BaseApiController
    {
        public DataContext _Context { get; }
        public UsersController(DataContext context)
        {
            _Context = context;
        }

         [HttpGet]
         [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           return await  _Context.Users.ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
         public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
         return await  _Context.Users.FindAsync(id);
        }


    }
}