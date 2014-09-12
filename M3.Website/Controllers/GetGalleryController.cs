using System.Web.Http;
using M3.Helpers;
using M3.Models;

namespace M3.Website.Controllers
{
    public class GetGalleryController : ApiController
    {
        // GET: api/GetGallery/Detail/5
        [HttpGet]
        public Category Detail(int id)
        {
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);
            return category;
        }

        // GET: api/GetGallery/Preview/5
        [HttpGet]
        public string Preview(int id)
        {
            return "value";
        }
    }
}
