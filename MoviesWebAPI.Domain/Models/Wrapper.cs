using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebAPI.Domain.Models
{

    public class Wrapper
    {
        [JsonProperty("Search")]
        public Search Search { get; set; }
    }
}
