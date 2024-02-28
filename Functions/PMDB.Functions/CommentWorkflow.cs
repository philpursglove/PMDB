using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;

namespace PMDB.Functions.API
{

    public static class CommentWorkflow
    {
        [Function("CommentApprovalWorkflow")]
        public static async Task CommentApprovalWorkflow([OrchestrationTrigger] TaskOrchestrationContext context)
        {

        }

        [Function("CommentWorkflowStart")]
        public static async Task<HttpResponseData> CommentWorkflowStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "/Comment")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext,
            [FromBody] CommentMessage comment)
        {
            await client.ScheduleNewOrchestrationInstanceAsync(nameof(CommentApprovalWorkflow));
        }
    }
}
