using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab3Movie.Models;
using Lab3Movie.Services;
using Lab3Movie.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3Movie.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService _userService;

        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = _userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = _userService.Register(registerModel);
            if (user == null)
            {
                return BadRequest(new { ErrorMessage = "Username already exists." });
            }
            return Ok(user);
        }

        
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A list of all users</returns>
        [HttpGet]
        [Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserGetModel> GetAll()
        {
            return _userService.GetAll();

        }


        /// <summary>
        /// Find an user by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /users
        ///     {  
        ///        id: 3,
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The user with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Users/5
        [Authorize(Roles = "Admin,UserManager")]
        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var found = _userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new User
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /users
        ///     {
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <param name="userPostModel">The input user to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        public void Post([FromBody] UserPostModel userPostModel)
        {
            _userService.Create(userPostModel);
        }



        /// <summary>
        /// Modify an user if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">id-ul user to update</param>
        /// <param name="userPostModel">obiect userPostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /users/id
        ///     {
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        userRole = "regular"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
            var result = _userService.Upsert(id, userPostModel);
            return Ok(result);
        }



        /// <summary>
        /// Delete an user
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _userService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id not fount !");
            }
            return Ok(result);
        }
    }
}