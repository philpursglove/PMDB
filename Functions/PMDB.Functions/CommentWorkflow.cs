using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using System.Net;

namespace PMDB.Functions.API
{

    public static class CommentWorkflow
    {
        [Function("CommentApprovalWorkflow")]
        public static async Task CommentApprovalWorkflow([OrchestrationTrigger] TaskOrchestrationContext context, CommentMessage comment)
        {
            bool approval = await context.WaitForExternalEvent<bool>("CommentApproval");

            if (approval)
            {
                await context.CallActivityAsync("PublishComment", comment);
            }
        }

        [Function("CommentWorkflowStart")]
        public static async Task<HttpResponseData> CommentWorkflowStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "/Comment")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext,
            [Microsoft.Azure.Functions.Worker.Http.FromBody] CommentMessage comment)
        {
            await client.ScheduleNewOrchestrationInstanceAsync(nameof(CommentApprovalWorkflow), comment);

            var response = HttpResponseData.CreateResponse(req);
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        [Function("PublishComment")]
        [QueueOutput("comments")]
        public static async Task<CommentMessage> PublishComment([ActivityTrigger] CommentMessage comment)
        {
            return comment;
        }

        [Function("CommentApproval")]
        public static async Task<HttpResponseData> CommentApproval(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", "/CommentApproval")]
            HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            await client.RaiseEventAsync(req.Query["workflowId"], "CommentApproval", true);

            var response = HttpResponseData.CreateResponse(req);
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }
    }
}
