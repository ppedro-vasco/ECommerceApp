using System.Security.Claims;
using ECommerceApp.Data;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _context.Carts
            .Where(c => c.UserId == userId)
            .Include(c => c.Product)
            .ToList();

            return View(cart);
        }

        public IActionResult Details(int id)
        {
            var carts = _context.Carts.Find(id);
            if (carts == null)
                return NotFound();

            return View(carts);
        }

        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = new SelectList(_context.Products, "ProductId", "Name");
                return View(cart);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // captura id do usuario logado
            
            if (userId == null)
                return Unauthorized();

            cart.UserId = userId;

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart == null)
                return NotFound();
            
            ViewBag.Products = new SelectList(_context.Products, "ProductId", "Name");
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cart cart)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartBase = await _context.Carts.FindAsync(cart.CartId);
            if (cartBase == null || cartBase.UserId != userId)
                return NotFound();
            
            cartBase.UserId = userId;
            cartBase.ProductId = cart.ProductId;
            cartBase.Quantity = cart.Quantity;

            _context.Carts.Update(cartBase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartBase = await _context.Carts.FindAsync(id);
            if (cartBase == null)
                return NotFound();

            _context.Carts.Remove(cartBase);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));  
        }
    }
}