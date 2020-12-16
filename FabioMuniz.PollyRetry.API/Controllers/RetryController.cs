using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FabioMuniz.PollyRetry.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetryController : ControllerBase
    {
        private readonly IHttpClientFactory _factory;
        public RetryController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/aspnet/AspNetCore.Docs/branches");
            
            request.Headers.Add("Accept", "application/vnd.github.v3+json");
            request.Headers.Add("User-Agent", "HttpClientFactory-PollyRetry");

            var client = _factory.CreateClient("polly-retry");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return Ok(JsonConvert.SerializeObject(response.Content.ReadAsStringAsync()));
            else
                return BadRequest();

        }
    }
}
