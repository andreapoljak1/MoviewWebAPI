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
            var result = _context.Movies.FirstOrDefault(s => s.Id == id);
            if (result != null)
            {
                _context.Movies.Remove(result);
                _context.SaveChanges();
            }
           
            return null;
        }

        public IEnumerable<Movie> QueryStringFilter(string s, string orderby,int per_page)
        {
            //može ovako ali može i bolje
            //List<Movie> result = new List<Movie>();
            //if (orderby == "asc")
            //{
            //    result = _context.Movies.Where(p => p.Title.Contains(s)).OrderBy(s => s.Id).Take(per_page).ToList();
            //}
            //else
            //{
            //   result = _context.Movies.Where(p => p.Title.Contains(s)).OrderByDescending(s => s.Id).Take(per_page).ToList();
            //}

            var result = _context.Movies.Where(p => p.Title.Contains(s)).ToList();
            switch (orderby)
            {
                case "desc":
                    result=result.OrderByDescending(p => p.Id).ToList();
                    break;
                default:
                    result=result.OrderBy(p => p.Id).ToList();
                    break;
            }
            if (per_page > 0)
            {
                result=result.Take(per_page).ToList();
            }



            return result;

        }








    }
}
