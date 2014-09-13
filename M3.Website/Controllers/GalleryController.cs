using System.Collections.Generic;
using System.Web.Http;
using M3.Configurations;
using M3.Helpers;
using M3.Models;

namespace M3.Website.Controllers
{
    public class GalleryController : ApiController
    {
        // GET: api/Gallery/Detail/1/3
        [HttpGet]
        public Category Detail(int id, int page, string callback)
        {
            return GalleryHelper.GetPagedCategory(id, page);
        }

        // GET: api/Gallery/Details/1/3
        [HttpGet]
        public List<Category> Details(string ids, int page)
        {
            var categories = new List<Category>();

            var idList = StringHelper.GetIntList(ids);

            foreach (var id in idList)
            {
                categories.Add(GalleryHelper.GetPagedCategory(id, page));
            }
            return categories;
        }

        // GET: api/Gallery/Preview/5
        [HttpGet]
        public Category Preview(int id)
        {
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);

            var previewSize = ConfigurationManager.WebsiteConfiguration.PreviewCategorySize <= category.Photos.Count ?
                ConfigurationManager.WebsiteConfiguration.PreviewCategorySize : category.Photos.Count;

            var previewCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year
            };

            previewCategory.Photos = ListHelper.GetRandomOrder<Photo>(category.Photos).GetRange(0, previewSize);

            return previewCategory;
        }
    }
}
