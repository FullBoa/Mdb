using System.Collections.Generic;
using Mdb.Logic.Stores;
using Microsoft.AspNetCore.Mvc;

namespace Mdb.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        private readonly IStore _store;

        public KeysController(IStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Gets existed keys.
        /// </summary>
        /// <returns>List of existed keys.</returns>
        [HttpGet]
        public ICollection<string> Get()
        {
            return _store.Keys();
        }
    }
}