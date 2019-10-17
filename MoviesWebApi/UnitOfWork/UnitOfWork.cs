using System;
using MoviesWebApi.Repositories;
using MoviesWebAPI.Domain;
using MoviesWebAPI.Domain.Models;

namespace MoviesWebApi.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        private MovieContext context = new MovieContext();
        private GenericRepository<Movie> movieRepository;

        public GenericRepository<Movie> MovieRepository
        {
            get
            {
                if (this.movieRepository == null)
                {
                    this.movieRepository = new GenericRepository<Movie>(context);
                }

                return movieRepository;
            }
        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
