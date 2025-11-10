using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var categories = await _categoryService.GetAllAsync();
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
                return View(product);
            }
                
            await _productService.CreateAsync(product);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            var categories = await _categoryService.GetAllAsync();

            if (product == null)
                return NotFound();

            ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            var productDb = await _productService.GetByIdAsync(product.ProductId);
            var categories = await _categoryService.GetAllAsync();

            if (productDb == null)
            {
                ViewBag.Categories = new SelectList(categories, "CategoryId", "Name");
                return NotFound();
            }

            productDb.Name = product.Name;
            productDb.Price = product.Price;
            productDb.Description = product.Description;
            productDb.CategoryId = product.CategoryId;
            productDb.UserId = product.UserId; //Seller

            await _productService.UpdateAsync(productDb);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDb = await _productService.GetByIdAsync(id);
            if (productDb == null)
                return NotFound();

            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}