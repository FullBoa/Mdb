using System.Collections.Generic;
using System.Net;
using Mdb.Logic.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Mdb.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IStore _store;

        public ValuesController(IStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Gets value by key if exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Value of key if exists.</returns>
        [HttpGet("{key}")]
        public string Get(string key)
        {
            return _store.Get(key);
        }

        /// <summary>
        /// Sets value by key if exists.
        /// </summary>
        /// <param name="pair">Pair for saving to store.</param>
        [HttpPost]
        public IActionResult Post([FromBody] KeyValuePair<string, string> pair)
        {
            if (string.IsNullOrWhiteSpace(pair.Key))
            {
                return BadRequest();
            }

            _store.Set(pair.Key, pair.Value);
            return Ok();
        }

        /// <summary>
        /// Deletes value by key.
        /// </summary>
        /// <param name="key">Key for removing.</param>
        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            var ok = _store.Delete(key);
            if (!ok)
                return NotFound();

            return Ok();
        }
    }
}