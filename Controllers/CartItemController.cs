using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class CartItemController : Controller
    {
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(int cartId, int productId, int quantity)
        {
            var items = await _cartItemService.GetItemsByCartIdAsync(cartId);

            var existingItem = items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                await _cartItemService.UpdateQuantityAsync(
                    existingItem.CartItemId,
                    existingItem.Quantity + quantity
                );
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                };

                await _cartItemService.AddAsync(newItem);
            }

            return RedirectToAction("Details", "Cart", new { id = cartId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int newQuantity)
        {
            var item = await _cartItemService.GetByIdAsync(cartItemId);

            if (item != null)
            {
                await _cartItemService.UpdateQuantityAsync(cartItemId, newQuantity);
                return RedirectToAction("Details", "Cart", new { id = item.CartId });
            }

            return NotFound();

        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var item = await _cartItemService.GetByIdAsync(itemId);

            if (item != null)
            {
                int cartId = item.CartId;
                await _cartItemService.DeleteAsync(itemId);

                return RedirectToAction("Details", "Cart", new { id = cartId });
            }
            
            return NotFound();
        }
    }
}