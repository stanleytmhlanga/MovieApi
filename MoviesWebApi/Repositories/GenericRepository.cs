using MoviesWebAPI.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal MovieContext context;
        internal DbSet<TEntity> dbset;


        public GenericRepository(MovieContext context)
        {
            this.context = context;
            this.dbset = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbset;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeproperties in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeproperties);
            }

            if (orderby != null)
            {
                return orderby(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        
        public virtual TEntity GetById(object id)
        {
            return dbset.Find(id);
        }

        public virtual async Task<IRestResponse> GetFromService(string parm)
        {
            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/?page=1&r=json&s=Avengers%20Endgame");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");
            IRestResponse response = client.Execute(request);

            return response;
        }
        public virtual async Task<IRestResponse> GetFromServiceByTitle(string parm)
        {
            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/?i="+parm+"&r=json");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");
            IRestResponse response = client.Execute(request);

            return response;
        }

        public virtual async Task<IRestResponse> SearchMovies(string parm)
        {
            //var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/?i=" + parm + "&r=json");

            var client = new RestClient("https://movie-database-imdb-alternative.p.rapidapi.com/?page=1&r=json&s="+parm+"");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "fc41884f5fmsh6181858ae897d1ep116698jsn4656feb5fd03");
            IRestResponse response = client.Execute(request);

            return response;
        }


        public virtual void Insert(TEntity entity)
        {
            dbset.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbset.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delet(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbset.Attach(entityToDelete);
            }

            dbset.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entity)
        {
            dbset.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}
