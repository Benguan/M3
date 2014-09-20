using System.Collections.Generic;

namespace M3.Models
{
    public class Gallery
    {
        public List<Category> Categories { get; set; }

        public int MaxId
        {
            get
            {
                int maxId = 0;
                if (Categories != null)
                {
                    Categories.ForEach(delegate(Category category)
                        {
                            if (category.Id > maxId)
                            {
                                maxId = category.Id;
                            }
                        });
                }
                return maxId;
            }
        }
    }
}
