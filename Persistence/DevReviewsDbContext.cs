using System.Collections.Generic;
using DevReviews.API.Entities;

namespace DevReviews.API.Persistence
{
    public class DevReviewsDbContext
    {
        public DevReviewsDbContext()
        {
            Products = new List<Product>(); // pega a lista de produtos e instacia uma nova lista de produtos
        }

        public List<Product> Products { get; set; } // cria lista de produtos
        
        
    }
}