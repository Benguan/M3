using System;
using System.Collections.Generic;
using System.Linq;
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
           
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);

			var previewSize = ConfigurationManager.WebsiteConfiguration.PreviewCategorySize >= category.Photos.Count ?
				ConfigurationManager.WebsiteConfiguration.PreviewCategorySize : category.Photos.Count;

            var previewCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year
            };

	        var lstPhotos = new List<Photo>();

			GetRandomPhotos(category.Photos.Count, previewSize, 0, category.Photos,ref lstPhotos);

	        previewCategory.Photos = lstPhotos.GetRange(0, previewSize);
  
            return previewCategory;
        }


		private void GetRandomPhotos(int allCount, int previewSize, int index, List<Photo> lstAllCategories, ref List<Photo> lstCategories)
	    {
		    if ( index >= previewSize) return;

			var random = new Random();
		    int randomIndex = random.Next(0, allCount);

		    if (lstCategories.Any(p=>p.Id == randomIndex))
		    {
				GetRandomPhotos(allCount, previewSize, index, lstAllCategories,ref lstCategories);
		    }
		    else
		    {
				lstCategories.Add(lstAllCategories.FirstOrDefault(p => p.Id == randomIndex));
				GetRandomPhotos(allCount, previewSize, ++index, lstAllCategories, ref lstCategories);
		    }
			
	    }
    }
}
