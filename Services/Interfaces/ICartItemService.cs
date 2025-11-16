using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;

namespace ECommerceApp.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<IEnumerable<CartItem>> GetItemsByCartIdAsync(int cartId);
        Task<CartItem> GetByIdAsync(int id);
        Task AddAsync(CartItem cartItem);
        Task UpdateQuantityAsync(int cartItemId, int quantity);
        Task DeleteAsync(int id);
        Task<Decimal> GetSubTotalAsync(int cartId);
    }
}