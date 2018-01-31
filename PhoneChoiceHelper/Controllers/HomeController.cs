
namespace PhoneChoiceHelper.Controllers
{
	using Dne.Core.Logging;
	using System.Web.Mvc;
    
	public class HomeController : Controller
    {
        private readonly ILogger logger;
        public HomeController(ILogger logger)
        {
            this.logger = logger;
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}