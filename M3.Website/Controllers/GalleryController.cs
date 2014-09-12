using System.Web.Http;
using M3.Configurations;
using M3.Helpers;
using M3.Models;

namespace M3.Website.Controllers
{
    public class GalleryController : ApiController
    {
        // GET: api/Gallery/Detail/5
        [HttpGet]
        public Category Detail(int id, int page)
        {
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);
            var pageSize = ConfigurationManager.WebsiteConfiguration.DetailPageSize;
            var getCount = pageSize;
            var startId = pageSize * (page - 1);
            var maxPage = category.Photos.Count / pageSize;
            if (page > maxPage)
            {
                getCount = category.Photos.Count - pageSize * (page - 1);
            }
            if (getCount < 0)
            {
                return null;
            }
            var pagedCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year,
                Photos = category.Photos.GetRange(startId, getCount)
            };

            return pagedCategory;
        }

        // GET: api/Gallery/Preview/5
        [HttpGet]
        public Category Preview(int id)
        {
            var previewSize = ConfigurationManager.WebsiteConfiguration.PreviewCategorySize;
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);
            var previewCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year
            };
            if (category.Photos.Count >= previewSize)
            {
                previewCategory.Photos = category.Photos.GetRange(0, previewSize);
            }
            else
            {
                previewCategory.Photos = category.Photos.GetRange(0, category.Photos.Count);
            }
            return previewCategory;
        }
    }
}
