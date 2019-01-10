using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace TodoFunction
{
    public static class QueueTriggers
    {
        [FunctionName("QueueTriggers")]
        public static void Run([QueueTrigger("todo", Connection = "AzureWebJobsStorage")]ToDo myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
