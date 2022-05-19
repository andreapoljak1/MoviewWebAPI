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
        public MoviesController(IMovieRepository movieRepository)
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
        //GET: api/movies/5
        [HttpGet("{id}")]
        public IActionResult FindMovies(int id)
        {
            try
            {
                var movie = _movieRepository.GetMovieById(id);
                if (movie == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Rezultat nije pronađen");
                }
                return Ok(movie);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Nije moguće prikazati rezultate, dogodila se greška!");
            }

        }

        //POST: api/Movies
        [HttpPost]
        public ActionResult PostMovie(Movie new_movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //Napomena:svojstvo primarnog ključa nije auto increment!
                //programsko rješenje za provjeru max vrijednosti svojstva id zapisa u tablivi te uvećavanje za 1 prije kreiranja novog zapisa
                var created_movie = _movieRepository.InsertMovie(new_movie);
                return Ok("Zapis je kreiran");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }


        //PUT: api/movies/5

        [HttpPut("{id}")]
        public ActionResult PutMovie(int id, Movie update_movie)
        {
            try
            {
                
                if (id != update_movie.Id)
                {
                    return BadRequest("Parametri ID se ne poklapaju");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Podaci nisu validni!");
                }
                var idExist = _movieRepository.GetMovieById(id);
                if (idExist == null)
                {
                    return NotFound("Zapis nije pronađen");
                }

                return Ok(_movieRepository.UpdateMovie(update_movie));


            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Greška kod ažuriranja podataka");
            }
           
        }

        ////DELETE:api/movies/5
        [HttpDelete("{id}")]
        public ActionResult DeleteMovie(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id nije dobar");
                }
                var movieDelete = _movieRepository.DeleteMovie(id);
                return Ok("Zapis je obrisan");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }


        //GET: api/movies/search
        [HttpGet("search")]
        public ActionResult SearchByQueryString(string s, string orderby="asc", int per_page=0)
        {
            try
            {
                return Ok(_movieRepository.QueryStringFilter(s, orderby, per_page));
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Greška u dohvatu podataka");
            }
        }

    }
}
