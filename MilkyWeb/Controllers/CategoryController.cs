using Microsoft.AspNetCore.Mvc;
using MilkyWeb.Data;
using MilkyWeb.Models;

namespace MilkyWeb.Controllers
{
    public class CategoryController : Controller // defined a new class inherits from controller class of asp.net
    {

        private readonly ApplicationDbContext _db;     // private field called _db will receive the instance of Applicationdbcontext
        public CategoryController(ApplicationDbContext db) //takes the instance of applicationdbcontext as parameter 
            //and assigns it to _db
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Category.ToList(); //to retrieve a list of categories
			// This line retrieves a list of categories from the database and assigns it to a local variable objCategoryList
			return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //used to handle form submissions and other data modifications.
		public IActionResult Create(Category obj)
		{
			// Check if the category name already exists
			if (_db.Category.Any(c => c.name == obj.name)) //to see if a category with the same name already exists in the database.
			{
				ModelState.AddModelError("name", "Category name already exists.");
			}

			// Check if the display order already exists
			if (_db.Category.Any(c => c.displayOrder == obj.displayOrder))
			{
				ModelState.AddModelError("displayOrder", "Display order already exists.");
			}

			if (ModelState.IsValid)  //check validations
            { 
            _db.Category.Add(obj); //add object to database catagory
            _db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Edit(int? id)
		{
			if(id==null || id==0)
			{
				return NotFound();
			}


			Category CategoryFromDb = _db.Category.Find(id); // Attempts to find a category using id

			if (CategoryFromDb == null)
			{
				return NotFound();
			}
			return View(CategoryFromDb); // If the category is found, it returns a view for editing the category,
		}

		[HttpPost] //used to handle form submissions and other data modifications.
		public IActionResult Edit(Category obj)
		{
			if (ModelState.IsValid)  //check validations
			{
				_db.Category.Update(obj); //add object to database catagory
				_db.SaveChanges();
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


			Category CategoryFromDb = _db.Category.Find(id); // Attempts to find a category using id

			if (CategoryFromDb == null)
			{
				return NotFound();
			}
			return View(CategoryFromDb); // If the category is found, it returns a view for editing the category,
		}

		[HttpPost, ActionName("Delete")] //used to handle form submissions and other data modifications.
		public IActionResult DeletePOST(int? id)
		{

			Category obj = _db.Category.Find(id);
			if(obj== null)
			{
				return NotFound();
			}
			_db.Category.Remove(obj);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
