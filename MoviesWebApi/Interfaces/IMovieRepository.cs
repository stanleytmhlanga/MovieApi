using MoviesWebAPI.Domain.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Interfaces
{
    public interface IMovieRepository
    {

        Task<IEnumerable<Movie>> GetMovies();
        void Update(Movie movie);
        void Insert(Movie movie);
        void Delete(int movieid);
        Task<Movie> GetById(int movieid);
        Task<IRestResponse> GetFromServiceById(string parm);
        Task<Movie> SearchMovieById(string parm);
        Task<IRestResponse> SearchMovies(string parm);
        Task<Wrapper> SearchMoviesByTitle(string parm);
        void Save();


    }
}
