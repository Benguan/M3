using System.Collections.Generic;
using System.Web.Http;

namespace M3.Website.Controllers
{
    public class GetGalleryController : ApiController
    {
        // GET: api/GetGallery
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/GetGallery/5
        public string Get(int id)
        {
            return "value";
        }

    }
}
