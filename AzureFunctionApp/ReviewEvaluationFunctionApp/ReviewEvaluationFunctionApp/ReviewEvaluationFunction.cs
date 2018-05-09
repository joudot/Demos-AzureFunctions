using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Rest;

namespace ReviewEvaluationFunctionApp
{
    public static class ReviewEvaluationFunction
    {
        [FunctionName("ReviewEvaluationFunction")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "Your_database_name",
            collectionName: "Review",
            ConnectionStringSetting = "CosmosDbConnectionString",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> documents,

            [DocumentDB(databaseName: "Your_database_name",
            collectionName: "Review",
            ConnectionStringSetting = "CosmosDbConnectionString",
            CreateIfNotExists = false)]IAsyncCollector<dynamic> results,

            TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.Verbose($"Documents modified  {documents.Count}");
                log.Verbose($"First document Id { documents[0].Id}");

                ITextAnalyticsAPI client = new TextAnalyticsAPI();

                client.AzureRegion = AzureRegions.Westcentralus;
                client.SubscriptionKey = "<Your_Subscription_Key>";

                string languageToAnalyze = "en";
                int cnt = 0;
                foreach (var document in documents)
                {
                    if (!string.IsNullOrEmpty(document.GetPropertyValue<string>("Satisfaction")))
                        continue;
                    var content = document.GetPropertyValue<string>("Content");
                    SentimentBatchResult result = client.Sentiment(
                        new MultiLanguageBatchInput(
                            new List<MultiLanguageInput>
                            {
                                new MultiLanguageInput(languageToAnalyze, id: cnt.ToString(), text: content)
                            }
                         )
                     );
                    cnt++;
                    var evaluationResult = result.Documents[0].Score;
                    var newDocument = new
                    {
                        id = document.Id,
                        Content = content,
                        Satisfaction = evaluationResult
                    };

                    await results.AddAsync(newDocument);
                    log.Verbose($"Review evaluated: {content}");
                    log.Verbose($"Evaluation result: {evaluationResult}");

                }
            }
        }
    }
}
