using System.Web.Mvc;

namespace M3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Wall()
        {
            return View();
        }

        public ActionResult Album()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}