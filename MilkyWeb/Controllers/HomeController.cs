using Microsoft.AspNetCore.Mvc;
using MilkyWeb.Models;
using System.Diagnostics;

namespace MilkyWeb.Controllers
{
    public class HomeController : Controller //defining a class name homecontroller that inherits from the controller class of asp.net
    {
        private readonly ILogger<HomeController> _logger; // Declares a read only field called _logger of type Ilogger<T>.
        // This field is intended to store an instance of a logger specific to the homecontroller class,
        //It allows homecontroller class to log messages and events based on its operation.

        public HomeController(ILogger<HomeController> logger) //contructor of homecontroller; takes in ilogger<t> as parameter and the 
            //parameter name is called logger when the instance of ilogger is created it will pass it to logger variable
            //assigns it to _logger field
        {
            _logger = logger; //dependency injection
        }

        public IActionResult Index() //handle requests to root url
        {
            return View();
        }

        public IActionResult Privacy() //handle requests to privacy ur; Home/privacy
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//used to explicitly disable caching for the specific action method to which it is applied.
		public IActionResult Error() //an action to handle error responses
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}