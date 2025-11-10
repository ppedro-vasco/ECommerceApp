using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders {get; set;}              
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Review> Reviews { get; set; }  
    }
}