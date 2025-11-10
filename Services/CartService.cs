using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Data;
using ECommerceApp.Models;
using ECommerceApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task CreateAsync(Cart cart)
        {
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if(cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            var carts = await _context.Carts
            .Include(c => c.User)
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();

            return carts;
        }

        public async Task<Cart> GetByIdAsync(int id)
        {
            var cart = await _context.Carts
            .Include(c => c.User)
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.CartId == id);

            return cart;
        }

        public async Task<decimal> GetTotalAsync(int cartId)
        {
            var cartItems = await _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CartId == cartId)
            .ToListAsync();

            var total = cartItems.Sum(item => item.Product.Price * item.Quantity);
            return total;
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}