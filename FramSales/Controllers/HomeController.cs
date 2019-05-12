using FramSales.Models;
using System.Web.Mvc;

namespace FramSales.Controllers
{

	public class HomeController : Controller
	{

		public ActionResult Index()
		{
			ViewBag.Title = "Login";

			return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			ViewBag.Title = "Login";
		
			return View();
		}

		public ActionResult LoginError()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginModel loginModel)
		{
			ViewBag.Title = "Login";

			return RedirectToAction("Login", "Contacts", loginModel);
		}
	} 
}
