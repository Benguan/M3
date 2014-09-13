using System.Web.Mvc;

namespace M3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Wall()
        {
            return View();
        }

        public ActionResult Album(int categoryId = 1, int photoId = 1)
        {
            ViewData["CategoryId"] = categoryId;
            ViewData["PhotoId"] = photoId;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}