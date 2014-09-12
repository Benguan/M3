
namespace M3.Helpers
{
    public class StringHelper
    {
        public static string GetUriFromPath(string path)
        {
            return path.Replace(@"\", "/");
        }
    }
}
