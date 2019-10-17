
using MoviesWebApi.Interfaces;
using domain = MoviesWebAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoviesWebAPI.Domain;
using MoviesWebAPI.Domain.Models;
using System.Data.Entity;
using RestSharp;
using RestSharp.Deserializers;

namespace MoviesWebApi.Repositories
{
    public class MovieRepository : ClientRepository,IMovieRepository
    {
        private MovieContext context;

        public MovieRepository(MovieContext context, ICacheService cache, IDeserializer serializer, ILogger logger) 
            : base(cache, serializer, logger, "https://movie-database-imdb-alternative.p.rapidapi.com/")
        {
            this.context = context;
        }
        public void Delete(int movieid)
        {
            Movie movie = context.Movies.Find(movieid);
            context.Movies.Remove(movie);
            Save();
        }

        public async Task<Movie> GetById(int movieid)
        {
            return await context.Movies.FindAsync(movieid);
        }

        public Task<IRestResponse> GetFromServiceById(string parm)
        {
            throw new NotImplementedException();
        }

        public async Task<Movie> SearchMovieById(string parm)
        {
            RestRequest requests = new RestRequest(BaseUrl+"?i="+parm +"&r=json", Method.GET);
            requests.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            requests.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");

            var response = GetFromCache<Movie>(requests, "Movie" + parm);

            return response;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await context.Movies.ToListAsync();
        }

        public void Insert(Movie movie)
        {
            context.Movies.Add(movie);
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task<IRestResponse> SearchMovies(string parm)
        {
            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/?page=1&r=json&s=" + parm + "");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");
            IRestResponse response = client.Execute(request);

            return response;
        }
        public async Task<Wrapper> SearchMoviesByTitle(string parm)
        {

            RestRequest requests = new RestRequest(BaseUrl + "?s=" + parm + "&r=json", Method.GET);
            requests.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            requests.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");

            var dd = Get<List<Wrapper>>(requests);
            var response = GetFromCache<Wrapper>(requests, "Movie" + parm);

            return response;
        }
        public void Update(Movie movie)
        {
            var RecordExist = context.Movies.Find(movie.Id);
            if (RecordExist == null)
            {
                context.Movies.Add(movie);
            }
            else
            {
                context.Entry(RecordExist).State = EntityState.Detached;
                context.Entry(movie).State = EntityState.Modified;
            }
        }

        
    }
}
