using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace SimpleQueue.WebApi.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class CallApiController : ControllerBase
    {
        public async Task<IActionResult> CallApi([FromQuery] string method, [FromQuery] string uri)
        {
            var url = HttpUtility.UrlDecode(uri);

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = string.Empty;

            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                RequestUri = new Uri(url)
            };

            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ToString();
            }

            return Ok(response);
        }
    }
}
