using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;

namespace ECommerceApp.Services.Interfaces
{
    public interface ICategoryService
    {
        Task CreateAsync(Category category);
        Task DeleteAsync(int id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task UpdateAsync(Category category);
    }
}