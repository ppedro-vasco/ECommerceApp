using System.Security.Claims;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _cartService.GetAllAsync();
            return View(carts);
        }

        public async Task<IActionResult> Details(int id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            ViewBag.Products = await _productService.GetAllAsync();
            return View(cart);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return View(cart);
            }

            cart.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartService.CreateAsync(cart);
            return RedirectToAction(nameof(Index));

        }

        #region Edit
        // [HttpGet]
        // public async Task<IActionResult> Edit(int id)
        // {
        //     var cart = await _cartService.GetByIdAsync(id);
        //     if (cart == null)
        //         return NotFound();

        //     return View(cart);
        // }

        // [HttpPost]
        // public async Task<IActionResult> Edit(Cart cart)
        // {
        //     // preciso de um edit para essa camada de cart? nao ficaria responsabilidade de um CartItemController?
        //     var cartDb = await _cartService.GetByIdAsync(cart.CartId);
        //     if (cartDb == null)
        //         return NotFound();

        //     cartDb.Items = cart.Items;

        //     await _cartService.UpdateAsync(cartDb);
        //     return RedirectToAction(nameof(Index));
        // }
        #endregion
        
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cart = await _cartService.GetByIdAsync(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartDb = await _cartService.GetByIdAsync(id);

            if (cartDb == null)
                return NotFound();
            
            await _cartService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}