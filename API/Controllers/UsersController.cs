using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //to get to this controller , api/users
    public class UsersController : ControllerBase
    {
        #region  Fields
        private readonly DataContext _context;
        #endregion

        #region Constructor
        public UsersController(DataContext context)
        {
            _context = context;
        }
        #endregion

        // #region Syncronous Code To get UserDetails
        // [HttpGet]
        // //<IEnumerable> is a simple list with no methods like search sort etc which are there for <List>
        // public ActionResult<IEnumerable<AppUser>> GetUsers()
        // {
        //     return _context.Users.ToList();
        // }

        // [HttpGet("{id}")]
        // //id is a required parameter api/users/2
        // public ActionResult<AppUser> GetUserById(int id)
        // {
        //     return _context.Users.Find(id);
        // }
        // #endregion

        #region Asyncronous Code To get UserDetails
        [HttpGet]
        //<IEnumerable> is a simple list with no methods like search sort etc which are there for <List>
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        //id is a required parameter api/users/2
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        #endregion
    }
}