﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Milky.DataAccess.Repository.IRepository;
using Milky.Models;
using Milky.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using Milky.Utility;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe.Checkout;

namespace MilkyWeb.Areas.Customer.Controllers
{
	[Area("Customer")]

	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public CartController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()
			};

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
		}

		public IActionResult Summary()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			ShoppingCartVM = new()
			{
				ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
				OrderHeader = new()
			};

			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
		}

		[HttpPost]
		[ActionName("Summary")]
		public IActionResult SummaryPOST()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

			//// Retrieve the submitted phone number from the model
			//var phoneNumber = ShoppingCartVM.OrderHeader.PhoneNumber;

			//ShoppingCartVM.OrderHeader.PhoneNumber = phoneNumber;

			var phoneNumberWith91 = "91" + ShoppingCartVM.OrderHeader.PhoneNumber;

			ShoppingCartVM.OrderHeader.PhoneNumber = phoneNumberWith91;


			ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

			ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
			ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
			ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

			_unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_unitOfWork.Save();

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				OrderDetail orderDetail = new()
				{
					ProductId = cart.ProductId,
					OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
					Price = cart.Price,
					Count = cart.Count,
				};
				_unitOfWork.OrderDetail.Add(orderDetail);
				_unitOfWork.Save();
			}

			//area for stripe
			//if(applicationUser.Id != null)
			var domain = "https://localhost:7276/";
			var options = new Stripe.Checkout.SessionCreateOptions
			{
				SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
				CancelUrl = domain + "Customer/Cart/Index",
				LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in ShoppingCartVM.ShoppingCartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100), //convert price to long 
						Currency = "INR",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.ProductName
						}
					},
					Quantity = item.Count
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new Stripe.Checkout.SessionService(); //create new session
			Session session = service.Create(options); //create new session with options

			_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult(303); // return redirect to url provided by stripe

		}

		public IActionResult OrderConfirmation(int id)
		{
			//id grabber code for authentication
			string currentUserID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

			if (orderHeader != null && orderHeader.ApplicationUserId == currentUserID)
			{

				if (orderHeader.PaymentStatus != null)
				{
					var service = new SessionService(); //create a new session class
					Session session = service.Get(orderHeader.SessionId); // pointing to the last session using session id

					if (session.PaymentStatus.ToLower() == "paid")
					{
						_unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
						_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);

						//generate and assign unique code if it's not already assigned

						if (string.IsNullOrEmpty(orderHeader.UniqueCode))
						{
							string uniqueCode = _unitOfWork.OrderHeader.GenerateUniqueCode();
							// Implement a method to update UniqueCode directly in the repository
							_unitOfWork.OrderHeader.UpdateUniqueCode(id, uniqueCode);
							_unitOfWork.Save();
							// Reload the orderHeader entity from the database

						}

						_unitOfWork.Save();


					}
					List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.
						GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList(); // gets the shopping cart list for specific user.
					_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
					_unitOfWork.Save();
				}
				OrderHeader orderHeader1 = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

				return View(new OrderConfirmationVM { OrderId = id, UniqueCodee = orderHeader1.UniqueCode });

				//return View(orderHeader.UniqueCode);
			}
			else
			{
				return NotFound();
			}
        }

		//private string GenerateUniqueCode()
		//{
		//	string uniqueCode;
		//	do
		//	{

		//		Random random = new Random();
		//		uniqueCode = new string(Enumerable.Range(0, 12).Select(_ => (char)('0' + random.Next(10))).ToArray());
		//		// creates a array of random code total of 12 digits and using iteration to create new random numbers
		//	}
		//	while ( //iterates loop if matching element is found inside order header
		//		_unitOfWork.OrderHeader.GetAll(u => u.UniqueCode == uniqueCode).Any());
		//	return uniqueCode;
		//}

		[HttpPost]
		public IActionResult UpdateQuantity(int itemId, int quantity)
		{
			try
			{
				var cartItem = _unitOfWork.ShoppingCart.Get(u => u.Id == itemId);
				if (cartItem == null)
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
			var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId); //finds the shopping cart entity based on id received from view
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

				var cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId);

				foreach (var cartItem in cartItems)
				{
					_unitOfWork.ShoppingCart.Remove(cartItem);
				}

				_unitOfWork.Save();

				TempData["success"] = "All items deleted successfully";
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
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
