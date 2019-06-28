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
using Microsoft.EntityFrameworkCore;

namespace Lab3Movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private IMovieService movieService;
        private IUsersService usersService;

        public MoviesController(IMovieService movieService, IUsersService usersService)
        {
            this.movieService = movieService;
            this.usersService = usersService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="page"></param>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /filme
        ///     {  id: 3,
        ///        title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: 2,
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///        comentarii: [
        ///            {
        ///                    id: 1,
        ///                    text: "film grozav",
        ///                    important: false
        ///             },
        ///             {
        ///                   id: 2,
        ///                   text: "film slab",
        ///                   important: false
        ///             }
        ///         ]
        ///     }
        ///
        ///          </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public PaginatedList<MovieGetModel> Get([FromQuery]DateTime? from, [FromQuery]DateTime? to,[FromQuery] int page = 1)
        {
            // TODO: make pagination work with /api/flowers/page/<page number>
            page = Math.Max(page, 1);
            return movieService.GetAll(page,from, to);
            //IQueryable<Movie> result = context.Movies.Include(c => c.Comments).OrderByDescending(m => m.YearOfRelease);
            //if (from == null && to == null)
            //{
            //    return result;
            //}
            //if (from != null)
            //{
            //    result = result.Where(m => m.DateAdded >= from);
            //}
            //if (to != null)
            //{
            //    result = result.Where(m => m.DateAdded <= to);
            //}
            //return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /filme
        ///     {  id: 3,
        ///        title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: 2,
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///        comentarii: [
        ///     {
        ///         id: 1,
        ///         text: "film grozav"
        ///     },
        ///     {
        ///         id: 2,
        ///         text: "film slab"
        ///     }
        ///     ]
        ///     }
        ///
        ///          </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Movies/5
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var found = movieService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }

            return Ok(found);
        }

        /// <summary>
        /// Adauga un film in baza de date
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     Post /filme
        ///      {  title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: "Action",
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///          comentarii: [
        ///     {
        ///         id: 1,
        ///         text: "film grozav"
        ///     },
        ///     {
        ///         id: 2,
        ///         text: "film slab"
        ///     }
        ///     ]        
        ///}
        ///</remarks>
        /// <summary>
        /// Add movie to database.
        /// </summary>
        /// <param name="movie">Movie to add.</param>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // POST: api/Movies
        [Authorize(Roles = "Admin,Regular")]
        [HttpPost]
        public void Post([FromBody] MoviePostModel movie)
        {
            User addedBy = usersService.GetCurentUser(HttpContext);
            //if (addedBy.UserRole == UserRole.UserManager)
            //{
            //    return Forbid();
            //}
            movieService.Create(movie, addedBy);
        }

        /// <summary>
        /// Update movie from database.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="movie"></param>
        /// Sample request:
        /// <remarks>
        ///     Post /filme
        ///      {  title: "The last fighter 2",
        ///        description: "povestea unui militar",
        ///        genre: "Action",
        ///        dateAdded: "2019-05-12T00:00:00",
        ///        duration: 120,
        ///        releaseYear: 2000,
        ///        director: "John Doe",
        ///        rating: 8,
        ///        watched: 1,
        ///}
        /// </remarks>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // PUT: api/Movies/5
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MoviePostModel movie)
        {
            var result = movieService.Upsert(id, movie);
            return Ok(result);
        }


        /// <summary>
        /// Delete movie from database.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // DELETE: api/ApiWithActions/5
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = movieService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
