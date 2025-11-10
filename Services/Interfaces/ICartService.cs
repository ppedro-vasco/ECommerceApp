using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;

namespace ECommerceApp.Services.Interfaces
{
    public interface ICartService
    {
        Task CreateAsync(Cart cart);
        Task DeleteAsync(int id);
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart> GetByIdAsync(int id);
        Task<decimal> GetTotalAsync(int cartId);
        Task UpdateAsync(Cart cart);
    }
}