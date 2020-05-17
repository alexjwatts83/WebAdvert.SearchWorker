using System.Threading.Tasks;
using AdvertApi.Models.Messages;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace WebAdvert.SearchWorker
{

    public class SearchWorker
    {
        private readonly IElasticClient _client;

        public SearchWorker():this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        { 
            // do call the other constructor
        }    
        public SearchWorker(IElasticClient client)
        {
            _client = client;
        }

        public async Task Function(SNSEvent snsEvent, ILambdaContext context)
        {
            foreach(var record in snsEvent.Records)
            {
                context.Logger.LogLine(record.Sns.Message);

                var advertConfirmedMessage = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
                var advertType = MappingHelper.Map(advertConfirmedMessage);

                var result = await _client.IndexDocumentAsync(advertType);
            }
        }
    }
}
