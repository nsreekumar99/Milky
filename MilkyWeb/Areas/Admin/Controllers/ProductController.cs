using Microsoft.AspNetCore.Mvc;
using Milky.DataAccess.Data;
using Milky.DataAccess.Repository.IRepository;
using Milky.Models;

namespace MilkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller // defined a new class inherits from controller class of asp.net
    {
        private readonly IUnitOfWork _unitOfWork;     // private field called _db will receive the instance of Applicationdbcontext
        public ProductController(IUnitOfWork unitOfWork) //takes the instance of applicationdbcontext as parameter 
                                                          //and assigns it to _db
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList(); //to retrieve a list of categories
                                                                                     // This line retrieves a list of categories from the database and assigns it to a local variable objProductList
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //used to handle form submissions and other data modifications.
        public IActionResult Create(Product obj)
        {
            // Check if the category name already exists
            if (_unitOfWork.Product.GetAll().Any(c => c.ProductName == obj.ProductName)) //to see if a category with the same name already exists in the database.
            {
                ModelState.AddModelError("name", "Product name already exists.");
            }

            if (ModelState.IsValid)  //check validations
            {
                _unitOfWork.Product.Add(obj); //add object to database catagory
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Product ProductFromDb = _unitOfWork.Product.Get(u => u.id == id); // Attempts to find a category using id

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb); // If the category is found, it returns a view for editing the category,
        }

        [HttpPost] //used to handle form submissions and other data modifications.
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)  //check validations
            {
                _unitOfWork.Product.Update(obj); //add object to database catagory
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            Product ProductFromDb = _unitOfWork.Product.Get(u => u.id == id); // Attempts to find a category using id

            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb); // If the category is found, it returns a view for editing the category,
        }

        [HttpPost, ActionName("Delete")] //used to handle form submissions and other data modifications.
        public IActionResult DeletePOST(int? id)
        {

            Product obj = _unitOfWork.Product.Get(u => u.id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
