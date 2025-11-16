using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly AppDbContext _context;
        public CartItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            await _context.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cartItems = await _context.CartItems.FindAsync(id);
            if (cartItems != null)
            {
                _context.CartItems.Remove(cartItems);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CartItem> GetByIdAsync(int id)
        {
            var cartItems = await _context.CartItems.FindAsync(id);
            if (cartItems == null)
                return null;

            return cartItems;
        }

        public async Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId)
        {
            var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();

            return cartItems;
        }

        public async Task<decimal> GetSubTotalAsync(int cartId)
        {
            var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();

            var total = cartItems.Sum(item => item.Product.Price * item.Quantity);
            return total;
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var cartItemToUpdate = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);

            if (cartItemToUpdate != null)
            {
                if (quantity <= 0)
                {
                    _context.CartItems.Remove(cartItemToUpdate);
                }
                else
                {
                    cartItemToUpdate.Quantity = quantity;
                }
                
                await _context.SaveChangesAsync();
            }
        }
    }
}