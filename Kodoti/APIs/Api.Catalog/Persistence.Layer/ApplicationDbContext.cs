using Domain.Layer;
using Microsoft.EntityFrameworkCore;
using Persistence.Layer.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Layer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ModelConfig(modelBuilder);
        }

        private void ModelConfig(ModelBuilder modelBuilder)
        {
            new ProductConfig(modelBuilder.Entity<Product>());
            new StoreConfig(modelBuilder.Entity<Store>());
            new ProductStoreConfig(modelBuilder.Entity<ProductStore>());
        }
    }
}
