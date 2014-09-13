
using System.Collections.Generic;
namespace M3.Helpers
{
    public class StringHelper
    {
        public static string GetUriFromPath(string path)
        {
            return path.Replace(@"\", "/");
        }

        public static List<int> GetIntPages(string pages)
        {
            var stringPages = pages.Split(',');
            var intPages = new List<int>();
            foreach (var page in stringPages)
            {
                int intPage;
                if (!string.IsNullOrEmpty(page))
                {
                    if (int.TryParse(page, out intPage))
                    {
                        intPages.Add(intPage);
                    }
                }
            }
            return intPages;
        }
    }
}
