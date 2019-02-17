using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MS.CatMatching.Entities;
using Newtonsoft.Json;

namespace MS.CatMatching.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        // GET api/cats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cat>>> GetAsync()
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("https://latelier.co")
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await client.GetAsync("/data/cats.json");

            if (response.IsSuccessStatusCode)
            {
                var cats = new List<Cat>();
                var data = await response.Content.ReadAsAsync<CatsImageCollection>();
                foreach (var cat in data.CatImages)
                {
                    cats.Add(new Cat
                    {
                        Id = 1,
                        Name = $"Cat - {cat.ExternalId}",
                        Image = cat
                    });
                }

                return Ok(cats);
            }
            else
            {
                //Something has gone wrong
            }
            return Ok();
        }
    }
}
