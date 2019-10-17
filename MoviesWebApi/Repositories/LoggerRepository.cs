using MoviesWebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Repositories
{
    public class LoggerRepository : ILogger
    {
        public void ErrorLog(Exception ex, string message)
        {
            //This will be used to log to db
            throw new NotImplementedException();
        }
    }
}
