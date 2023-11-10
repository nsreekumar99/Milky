using Microsoft.AspNetCore.Mvc;

namespace MilkyWeb.Controllers
{
	public class MilkController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
