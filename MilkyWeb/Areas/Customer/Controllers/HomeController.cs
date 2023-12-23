using Microsoft.AspNetCore.Mvc;
using Milky.DataAccess.Repository;
using Milky.DataAccess.Repository.IRepository;
using Milky.Models;
using System.Diagnostics;
using Milky.Models.ViewModels;

namespace MilkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller //defining a class name homecontroller that inherits from the controller class of asp.net
    {
        private readonly ILogger<HomeController> _logger; // Declares a read only field called _logger of type Ilogger<T>.
        // This field is intended to store an instance of a logger specific to the homecontroller class,
        //It allows homecontroller class to log messages and events based on its operation.
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) //contructor of homecontroller; takes in ilogger<t> as parameter and the 
                                                                                      //parameter name is called logger when the instance of ilogger is created it will pass it to logger variable
                                                                                      //assigns it to _logger field
        {
            _logger = logger; //dependency injection
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() //handle requests to root url
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
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