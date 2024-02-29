using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Milky.DataAccess.Repository.IRepository;
using Milky.Models;
using Milky.Models.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MilkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(IEnumerable<int> selectedCategories)
        {
            var allCategories = _unitOfWork.Category.GetAll().ToList();
            var allProducts = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

            // Apply category filter if selectedCategories is not null
            if (selectedCategories != null && selectedCategories.Any())
            {
                allProducts = allProducts.Where(p => selectedCategories.Contains(p.CategoryID)).ToList();
            }

            var productVM = new ProductVM
            {
                ProductList = allProducts,
                CategoryList = allCategories.Select(c => new SelectListItem { Value = c.id.ToString(), Text = c.name }),
                SelectedCategoryIds = selectedCategories?.ToList() ?? new List<int>()
            };

            ViewBag.Categories = allCategories;

            return View(productVM);
        }

        public IActionResult Buy(int id)
        {
            Product product = _unitOfWork.Product.Get(u => u.id == id, includeProperties: "Category");
            return View(product);   
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
