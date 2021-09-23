using System.Collections.Generic;
using DevReviews.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevReviews.API.Persistence
{
    public class DevReviewsDbContext : DbContext
    {
        public DevReviewsDbContext(DbContextOptions<DevReviewsDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; } // cria lista de produtos
        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            /* modelBuilder.Entity<Product>()
            .ToTable("tb_Product");

            modelBuilder.Entity<Product>()
            .HasKey(p => p.Id); */

            modelBuilder.Entity<Product>(p => {
                p.ToTable("tb_Product");
                p.HasKey(p => p.Id);

                p.HasMany(pp => pp.Reviews)
                 .WithOne()
                 .HasForeignKey(r => r.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            /* modelBuilder.Entity<ProductReview>()
            .ToTable("tb_ProductReview"); */

            modelBuilder.Entity<ProductReview>(pr => {
                pr.ToTable("tb_ProductReview");
                pr.HasKey(p => p.Id);

                pr.Property(p => p.Author) // restrições
                .HasMaxLength(50)
                .IsRequired(); // o atributo author vai ter no máximo 50 caracteres
            });
        }
        
        
        
        
    }
}