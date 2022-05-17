using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Data.Interfaces;
using Movies.Data.Models;
using Movies.Data.Repositories;

namespace Movies.API.Controllers
{
    //RUTA: localhost:8000/api/Movies
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        //konstruktor
        public MoviesController (IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }
        //GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            //moramo napraviti inection klase repozitorija
            try
            {
                return Ok(_movieRepository.GetAll());
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            
        }

        [Route("api/Movies")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult FindMovies(int id)
        {
            if (id == 0)
            {
                return BadRequest("Nije moguće prikazati rezultate, dogodila se greška!");
            }
            var movie = _movieRepository.GetMovieById(id);
            if (movie == null)
            {

                return NotFound("Rezultat nije pronađen");
            }
            else
            {
                return Ok(movie);
            }

        }

    }
}
