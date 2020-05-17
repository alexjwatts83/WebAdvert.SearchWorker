using Microsoft.Extensions.Configuration;
using Nest;

namespace WebAdvert.SearchWorker
{
    public static class ElasticSearchHelper
    {
        private static IElasticClient client;

        public static IElasticClient GetInstance(IConfiguration configuration)
        {
            if (client == null)
            {
                var url = configuration
                    .GetSection("ES")
                    .GetValue<string>("url");

                var settings = new ConnectionSettings(new System.Uri(url))
                    .DefaultIndex("adverts")
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));

                client = new ElasticClient(settings);
            }

            return client;
        }
    }
}
