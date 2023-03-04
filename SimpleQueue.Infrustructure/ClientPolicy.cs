using Polly;
using Polly.Retry;

namespace SimpleQueue.Infrastructure
{
    public class ClientPolicy
    {
        public AsyncRetryPolicy<HttpResponseMessage> RetryPolicy { get; set; }

        public ClientPolicy()
        {
            RetryPolicy = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode).RetryAsync(5);
        }
    }
}
