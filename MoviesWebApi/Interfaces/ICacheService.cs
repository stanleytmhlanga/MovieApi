using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string cache) where T : class;
        void Set(string cache, object item, int minutes);
    }
}
