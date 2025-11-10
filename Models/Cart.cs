using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        
        [ForeignKey("User")]
            public string UserId { get; set; }
        public User User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>(); // 1:N
    }
}