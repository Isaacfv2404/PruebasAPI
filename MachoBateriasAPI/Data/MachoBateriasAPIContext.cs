using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MachoBateriasAPI.Models;

namespace MachoBateriasAPI.Data
{
    public class MachoBateriasAPIContext : DbContext
    {
        public MachoBateriasAPIContext (DbContextOptions<MachoBateriasAPIContext> options)
            : base(options)
        {
        }

        public DbSet<MachoBateriasAPI.Models.Client> Client { get; set; } = default!;

        public DbSet<MachoBateriasAPI.Models.Vehicle>? Vehicle { get; set; }

        public DbSet<MachoBateriasAPI.Models.Warranty>? Warranty { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("product"); // Tabla para productos
            modelBuilder.Entity<Battery>().ToTable("battery"); // Tabla para baterías
            modelBuilder.Entity<Employee>().ToTable("employee"); 
            modelBuilder.Entity<Sale>().ToTable("sale");
            modelBuilder.Entity<Buys>().ToTable("buys");
            modelBuilder.Entity<Supplier>().ToTable("supplier");

            // Configuración de las columnas comunes (Id, Name, Description, Brand, Price, Stock, ProductType)
            modelBuilder.Entity<Product>()
                .HasKey(p => p.id);
            modelBuilder.Entity<Product>()
                .Property(p => p.name);
            modelBuilder.Entity<Product>()
                .Property(p => p.description);
            modelBuilder.Entity<Product>()
                .Property(p => p.brand);
            modelBuilder.Entity<Product>()
                .Property(p => p.price);
            modelBuilder.Entity<Product>()
                .Property(p => p.stock);
            modelBuilder.Entity<Product>()
                .Property(p => p.productType);

            // Configuración específica de la tabla Batteries
            modelBuilder.Entity<Battery>()
                .Property(b => b.model);
            modelBuilder.Entity<Battery>()
                .Property(b => b.capacity);
            modelBuilder.Entity<Battery>()
                .Property(b => b.voltage);
            modelBuilder.Entity<Battery>()
                .Property(b => b.type);
            modelBuilder.Entity<Battery>()
                .Property(b => b.weight);
            modelBuilder.Entity<Battery>()
                .Property(b => b.large); ;
            modelBuilder.Entity<Battery>()
                .Property(b => b.width);
            modelBuilder.Entity<Battery>()
                .Property(b => b.height);
            modelBuilder.Entity<Battery>()
                .Property(b => b.expiration);

            //employee
            modelBuilder.Entity<Employee>()
               .Property(e => e.identification);
            modelBuilder.Entity<Employee>()
                .Property(e => e.name);
            modelBuilder.Entity<Employee>()
                .Property(e => e.address);
            modelBuilder.Entity<Employee>()
                .Property(e => e.phone);
            modelBuilder.Entity<Employee>()
                .Property(e => e.email);


            //provedor
            modelBuilder.Entity<Supplier>()
            .Property(s => s.id);
            modelBuilder.Entity<Supplier>()
                .Property(s => s.name);
            modelBuilder.Entity<Supplier>()
                .Property(s => s.email);
            modelBuilder.Entity<Supplier>()
                .Property(s => s.address);
            modelBuilder.Entity<Supplier>()
                .Property(s => s.phone);
            modelBuilder.Entity<Supplier>()
                .Property(s => s.typeProduct);

            //
            modelBuilder.Entity<Buys>()
               .Property(b => b.id);
            modelBuilder.Entity<Buys>()
                .Property(b => b.date);
            modelBuilder.Entity<Buys>()
                .Property(b => b.supplierId);
            modelBuilder.Entity<Buys>()
                .Property(b => b.employeeId);
            modelBuilder.Entity<Buys>()
                .Property(b => b.total);



            ///  
            modelBuilder.Entity<Sale>()
                .Property(s => s.code);
            modelBuilder.Entity<Sale>()
                .Property(s => s.date);
            modelBuilder.Entity<Sale>()
                .Property(s => s.employeeId);
            modelBuilder.Entity<Sale>()
                .Property(s => s.clientId);
            modelBuilder.Entity<Sale>()
                .Property(s => s.discount);
            modelBuilder.Entity<Sale>()
                .Property(s => s.subTotal);
            modelBuilder.Entity<Sale>()
                .Property(s => s.total);




        }



        public DbSet<MachoBateriasAPI.Models.Product>? Product { get; set; }
        public DbSet<MachoBateriasAPI.Models.Battery>? Battery { get; set; }

        public DbSet<MachoBateriasAPI.Models.Employee>? Employee { get; set; }
        public DbSet<MachoBateriasAPI.Models.Sale>? Sale { get; set; }
        public DbSet<MachoBateriasAPI.Models.Supplier>? Supplier { get; set; }
        public DbSet<MachoBateriasAPI.Models.Buys>? Buys { get; set; }
        public DbSet<MachoBateriasAPI.Models.SaleProduct>? SaleProduct { get; set; }

        //consulta para los productos de cada factura
        public async Task<List<Product>> GetProductsForSaleAsync(int saleId)
        {
            // Utiliza LINQ para obtener los productos asociados a la venta específica
            var productIdsForSale = await SaleProduct
                .Where(sp => sp.saleId == saleId)
                .Select(sp => sp.productId)
                .ToListAsync();

            // Consulta los productos basados en los Ids obtenidos
            var productsForSale = await Product
                .Where(p => productIdsForSale.Contains(p.id))
                .ToListAsync();

            return productsForSale;
        }

        //consulta para los productos de cada factura
        public DbSet<MachoBateriasAPI.Models.Branch>? Branch { get; set; }

        //consulta para los productos de cada factura
        public DbSet<MachoBateriasAPI.Models.BuysProduct>? BuysProduct { get; set; }

        //consulta para los productos de cada factura
        public async Task<List<Product>> GetProductsForBuysAsync(int buysId)
        {
            var productIdsForBuys = await BuysProduct
                .Where(sp => sp.buysId == buysId)
                .Select(sp => sp.productId)
                .ToListAsync();

            // Consulta los productos basados en los Ids obtenidos
            var productsForBuys = await Product
                .Where(p => productIdsForBuys.Contains(p.id))
                .ToListAsync();

            return productsForBuys;
        }
    }
}
