﻿using System.Web.Http;
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
            var gallery = StorageHelper.GetGallery();
            var category = gallery.Categories.Find(c => c.Id == id);
            var pageSize = ConfigurationManager.WebsiteConfiguration.DetailPageSize;
            var photosCount = category.Photos.Count;
            var currentPageSize = pageSize;
            var startId = pageSize * (page - 1);
            if (startId > photosCount)
            {
                return null;
            }
            var maxFullPage = category.Photos.Count / pageSize;
            if (page > maxFullPage)
            {
                currentPageSize = category.Photos.Count - pageSize * (page - 1);
            }
            var pagedCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year,
                Page = page,
                Cover = category.Photos[0],
                Photos = category.Photos.GetRange(startId, currentPageSize)
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

            previewCategory.Photos = ListHelper.GetRandomOrder<Photo>(category.Photos).GetRange(0, previewSize);

            return previewCategory;
        }
    }
}