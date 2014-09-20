using System.Collections.Generic;
using M3.Configurations;
using M3.Models;

namespace M3.Helpers
{
    public class GalleryHelper
    {
        public static Category GetPagedCategory(int categoryId, int page, bool fill)
        {
            var categories = StorageHelper.GetAllCategories();

            var category = categories.Find(c => c.Id == categoryId);
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

            if (fill)
            {
                for (var i = 0; i < pageSize - currentPageSize; i++)
                {
                    pagedCategory.Photos.Add(null);
                }
            }
            return pagedCategory;
        }

        public static Category GetCategoryByPage(int categoryPage, int photoPage, bool fill)
        {
            var categories = StorageHelper.GetAllCategories();

            var category = categories.Find(c => c.Page == categoryPage);
            var pageSize = ConfigurationManager.WebsiteConfiguration.DetailPageSize;
            var photosCount = category.Photos.Count;
            var currentPageSize = pageSize;
            var startId = pageSize * (photoPage - 1);
            if (startId > photosCount)
            {
                return null;
            }
            var maxFullPage = category.Photos.Count / pageSize;
            if (photoPage > maxFullPage)
            {
                currentPageSize = category.Photos.Count - pageSize * (photoPage - 1);
            }
            var pagedCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                Year = category.Year,
                Page = category.Page,
                Cover = category.Photos[0],
                Photos = category.Photos.GetRange(startId, currentPageSize)
            };

            if (fill)
            {
                for (var i = 0; i < pageSize - currentPageSize; i++)
                {
                    pagedCategory.Photos.Add(null);
                }
            }
            return pagedCategory;
        }

    }
}
