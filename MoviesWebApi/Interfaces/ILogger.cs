using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Interfaces
{
    public interface ILogger
    {
        void ErrorLog(Exception ex, string message);
    }
}
