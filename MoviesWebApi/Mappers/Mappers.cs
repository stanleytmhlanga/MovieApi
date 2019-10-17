namespace MoviesWebApi.Mappers
{
    public static class Mappers
    {

        public static Models.Movie MapToApi(this MoviesWebAPI.Domain.Models.Movie domain)
        {
            Models.Movie api = new Models.Movie();
            if (domain != null)
            {
                api.Id = domain.Id;
                api.Title = domain.Title;
                api.Director = domain.Director;
                //api.ReleasedDate = domain.Released;
            }
            return api;
        }

        public static MoviesWebAPI.Domain.Models.Movie MapToDomain(this Models.Movie api)
        {
            MoviesWebAPI.Domain.Models.Movie domain = new MoviesWebAPI.Domain.Models.Movie();
            if (domain != null)
            {
                domain.Id = api.Id;
                domain.Title = api.Title;
                domain.Director = api.Director;
                //domain.Released= api.ReleasedDate;
            }
            return domain;
        }
    }
}
