using MoviesWebApi.Interfaces;
using MoviesWebApi.UnitOfWork;
using MoviesWebAPI.Domain.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MovieWebApplication.Controllers
{
    [System.Web.Http.RoutePrefix("api/Movie")]
    public class MovieController : ApiController
    {
        private UnitOfWork unitOfWork;
        private IMovieRepository repository;

        public MovieController(IMovieRepository repository, UnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        // GET: api/GetMovieById
        [ResponseType(typeof(IEnumerable<Movie>))]
        [Route("SearchMovieById")]
        [HttpGet]
        public async Task<HttpResponseMessage> SearchMovieById(string Id)
        {
            HttpResponseMessage response = null;
            var responseFromSwevice = await repository.SearchMovieById(Id);
            response = Request.CreateResponse(HttpStatusCode.OK, responseFromSwevice);

            return response;

        }

        // GET: api/GetMovieByTitle
        [ResponseType(typeof(IEnumerable<Movie>))]
        [Route("SearchByTitle")]
        [HttpGet]
        public async Task<HttpResponseMessage> SearchMoviesByTitle(string title)
        {
            HttpResponseMessage response = null;
            var responseFromSwevice = await repository.SearchMovies(title);
            if (responseFromSwevice.StatusCode == HttpStatusCode.OK && responseFromSwevice.IsSuccessful == true)
            {
                response = Request.CreateResponse(HttpStatusCode.OK, responseFromSwevice.Content);
            }
            else
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, response.RequestMessage);
            }

            return response;
        }
      
    }
}
