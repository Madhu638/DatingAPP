using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API.Entities;
using System.Linq;
using API.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsersController :ControllerBase
    {
        public DataContext _Context { get; }
        public UsersController(DataContext context)
        {
            _Context = context;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
           return await  _Context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
         public async Task<ActionResult<AppUser>> GetUsers(int id)
        {
         return await  _Context.Users.FindAsync(id);
        }


    }
}