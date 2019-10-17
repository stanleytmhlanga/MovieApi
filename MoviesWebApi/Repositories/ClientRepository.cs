using MoviesWebApi.Interfaces;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesWebApi.Repositories
{
    public class ClientRepository : RestSharp.RestClient
    {
        private ICacheService cache;
        private ILogger logger;
        public ClientRepository(ICacheService cache, IDeserializer serializer, ILogger logger, string baseUrl)
        {
            this.cache = cache;
            this.logger = logger;
            AddHandler("application/json", serializer);
            AddHandler("text/json", serializer);
            AddHandler("text/x-json", serializer);
           
            BaseUrl = new Uri(baseUrl);
        }


        public override IRestResponse Execute(IRestRequest restRequest)
        {
            var response = base.Execute(restRequest);
            CheckTimeout(response, restRequest);

            return response;
        }
        private void CheckTimeout(IRestResponse response, IRestRequest restRequest)
        {
            if (response.StatusCode == 0)
            {
                ErrorLog(response, restRequest, BaseUrl);
            }
        }
        private void ErrorLog(IRestResponse response, IRestRequest restRequest, Uri BaseUrl)
        {
            Exception exception;
            string parameters = string.Join(", ", restRequest.Parameters.Select(x => x.Name.ToString() + "=" + ((x.Value == null) ? "NULL" : x.Value)).ToArray());

            string info = "Request to " + BaseUrl.AbsoluteUri + restRequest.Resource + " failed with status code " + response.StatusCode + ", parameters: "
            + parameters + ", and content: " + response.Content;

            if (response != null && response.ErrorException != null)
            {
                exception = response.ErrorException;
            }
            else
            {
                exception = new Exception(info);
            }

        }
        public T Get<T>(IRestRequest restRequest) where T:new()
        {
            var response = base.Execute<T>(restRequest);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                ErrorLog(response, restRequest, BaseUrl);
                return default(T);
            }
        }
        public T GetFromCache<T>(IRestRequest restRequest, string cacheKey) where T : class, new()
        {
            var item= cache.Get<T>(cacheKey);
            
            if (item == null)
            {
                var response = Execute<T>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    cache.Set(cacheKey,response.Data,20);
                    item= response.Data;
                }
                else
                {
                    ErrorLog(response, restRequest, BaseUrl);
                    return default(T);
                }
            }
            return item;
        }
    }
}
