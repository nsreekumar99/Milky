using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milky.DataAccess.Repository.IRepository;
using Milky.Models;
using Milky.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MilkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]

    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == userId, includeProperties:"Product"),
			};

            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }
			return View(ShoppingCartVM);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int itemId, int quantity)
        {
            try
            {
                var cartItem = _unitOfWork.ShoppingCart.Get(u => u.Id == itemId);
                if (cartItem == null )
                {
                    return NotFound();
                }

                if (quantity < 1)
                {
                    _unitOfWork.ShoppingCart.Remove(cartItem);
                    _unitOfWork.Save();

					var claimsIdentity = (ClaimsIdentity)User.Identity;
					var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

					// Recalculate order total based on the updated quantity
					var newOrderTotal = RecalculateOrderTotal(cartItem.ApplicationUserId);


					ShoppingCartVM = new ShoppingCartVM
					{
						ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")
					};


					// Check if ShoppingCartList is not null before accessing Count
					var totalProductCount = ShoppingCartVM.ShoppingCartList != null ? ShoppingCartVM.ShoppingCartList.Count() : 0;

					return Json(new { itemId = itemId, totalProductCount = totalProductCount, newOrderTotal = newOrderTotal, deleted = true });

				}
                else
                {

                    cartItem.Count = quantity;
                    _unitOfWork.ShoppingCart.Update(cartItem);
                    _unitOfWork.Save();

					// Recalculate order total based on the updated quantity
					var newOrderTotal = RecalculateOrderTotal(cartItem.ApplicationUserId);

					// Get the updated count after saving changes
					var updatedCount = _unitOfWork.ShoppingCart.Get(u => u.Id == itemId)?.Count;

                    // Return JSON response with updated count
                    return Json(new { itemId = itemId, newCount = updatedCount, newOrderTotal = newOrderTotal, deleted = false });
                }
			}
			catch (Exception ex)
			{
				return StatusCode(500, "Internal server error: " + ex.Message);
			}
            return RedirectToAction(nameof(Index));
		}

		// Method to recalculate order total based on the user's shopping cart items
		private double RecalculateOrderTotal(string userId)
		{
			double orderTotal = 0;

			var shoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

			foreach (var cart in shoppingCartItems)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				orderTotal += (cart.Price * cart.Count);
			}

			return orderTotal;
		}


		public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u=>u.Id== cartId); //finds the shopping cart entity based on id received from view
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Product removed from cart successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearCart()
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                var cartItems = _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId ==userId );

				foreach (var cartItem in cartItems)
				{
					_unitOfWork.ShoppingCart.Remove(cartItem);
				}

                _unitOfWork.Save();

                TempData["success"] = "All items deleted successfully";
                return RedirectToAction(nameof(Index));
			}
            catch(Exception ex)
            {
                return StatusCode(500, "Internal Server Error" + ex.Message);
            }
        }
        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            return shoppingCart.Product.Price;
        }

    }
}
