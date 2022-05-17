using Movies.Data.Interfaces;
using Movies.Data.Models;

namespace Movies.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        //injektiramo clasu
        private readonly algebramssqlhost_moviesContext _context;

        //konstruktor
        public MovieRepository(algebramssqlhost_moviesContext context)
        {
            _context = context;
        }
        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }
        public Movie GetMovieById(int id)
        {
            return _context.Movies.FirstOrDefault(s=>s.Id==id);
        }
        public Movie InsertMovie(Movie new_movie)
        {
            var searchMovie = _context.Movies.FirstOrDefault(s => s.Id == new_movie.Id);
            if (searchMovie != null)
            {
                var max_id=_context.Movies.Max(s=>s.Id);
                new_movie.Id= max_id+1;
            }
           var result= _context.Movies.Add(new_movie);
            _context.SaveChanges();
            return result.Entity;
        }
        public Movie UpdateMovie(Movie new_movie)
        {
            //var result1 = GetMovieById(new_movie.Id);
            var result = _context.Movies.FirstOrDefault(s => s.Id == new_movie.Id);
            result.Title=new_movie.Title;
            result.Genre=new_movie.Genre;
            result.ReleaseYear=new_movie.ReleaseYear;
           
            _context.SaveChanges();
            return result;
        }
        public Movie DeleteMovie(int id)
        {
            return null;
        }

        public IEnumerable<Movie> QueryStringFilter(string s, string orderby,int per_page)
        {
            return null;
        }








    }
}
