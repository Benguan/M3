using System.Web.Http;

namespace M3.Website.Controllers
{
    public class GetGalleryController : ApiController
    {
        // GET: api/GetGallery/Detail/5
        [HttpGet]
        public string Detail(int id)
        {
            return "value";
        }

        // GET: api/GetGallery/Preview/5
        [HttpGet]
        public string Preview(int id)
        {
            return "value";
        }
    }
}
