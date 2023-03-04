using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleQueue.Infrastructure;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CallApiController : ControllerBase
    {
        private readonly ClientPolicy _clientPolicy;
        public CallApiController(ClientPolicy clientPolicy)
        {
            _clientPolicy = clientPolicy;
        }
        public async Task<IActionResult> CallApiAsync([FromQuery] string method, [FromQuery] string uri)
        {
            var url = HttpUtility.UrlDecode(uri);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url)
            };

            HttpResponseMessage response = await _clientPolicy.RetryPolicy.ExecuteAsync(
                () => client.SendAsync(request));

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }

            return BadRequest(response.StatusCode);
        }
    }
}
