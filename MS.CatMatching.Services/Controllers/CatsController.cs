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
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        // GET api/cats
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cat>>> GetAsync()
        {
            var random = new Random(100);

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
                        Image = cat,
                        Votes = RandomNumber(1, 100)
                    });
                }

                return Ok(cats.OrderByDescending(x => x.Votes));
            }
            else
            {
                //Something has gone wrong
            }
            return Ok();
        }

        private async Task<IEnumerable<Cat>> GetByIdAsync(int id)
        {
            var random = new Random(100);

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
                        Id = cats.Count + 1,
                        Name = $"Cat - {cat.ExternalId}",
                        Image = cat,
                        Votes = RandomNumber(1, 100)
                    });
                }

                return cats.Where(x => x.Id == id).OrderByDescending(x => x.Votes);
            }
            return new Cat[] { };
        }
        // GET api/cats/random
        [HttpGet]
        [Route("random")]
        public async Task<ActionResult<IEnumerable<Cat>>> GetRandomAsync()
        {
            var first = await GetByIdAsync(RandomNumber(1, 100));
            var second =await GetByIdAsync(RandomNumber(1, 100));

            return Ok(new Cat[] { first.FirstOrDefault(), second.FirstOrDefault() });
        }

        // GET api/cats/random
        [HttpPost]
        [Route("vote")]
        public async Task<ActionResult> PostVoteAsync([FromBody] int id)
        {
            return Ok();
        }
    }
}
