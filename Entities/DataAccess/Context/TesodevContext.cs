using System;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Models;

namespace Shared.DataAccess.Context
{
    public partial class TesodevContext : DbContext
    {
        private readonly IOptions<ConnectionStrings> _config;

        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {   
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)
                .AddConsole();
        });

        public TesodevContext(DbContextOptions<TesodevContext> options, IOptions<ConnectionStrings> config)
            : base(options)
        {
            _config = config;        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
            .UseLoggerFactory(_loggerFactory)
            .EnableSensitiveDataLogging()
            //.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information
            .UseMySql(_config.Value.CustomerDatabase, ServerVersion.AutoDetect(_config.Value.CustomerDatabase))
            ;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));


                entity.HasMany(x => x.Orders)
                    .WithOne(x => x.Address)
                    .HasForeignKey(x => x.AddressId);

                entity.HasMany(x => x.Customers)
                    .WithOne(x => x.Address)
                    .HasForeignKey(x => x.AddressId);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));


                entity.Property(e => e.AddressId)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));
                entity.HasOne(x => x.Address)
                    .WithMany(x => x.Customers)
                    .HasForeignKey(x => x.AddressId);

                entity.HasMany(x => x.Orders)
                    .WithOne(x => x.Customer)
                    .HasForeignKey(x => x.CustomerId);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));


                entity.Property(e => e.AddressId)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));
                entity.HasOne(x => x.Address)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.AddressId);


                entity.Property(e => e.ProductId)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));
                entity.HasOne(x => x.Product)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.ProductId);


                entity.Property(e => e.CustomerId)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));
                entity.HasOne(x => x.Customer)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(x => x.CustomerId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.HasKey(x => x.Id);
                entity.Property(e => e.Id)
                    .HasColumnType("VARCHAR(36)")
                    .HasConversion(
                        id => id.ToString(),
                        idString => Guid.Parse(idString));

                entity.HasMany(x => x.Orders)
                    .WithOne(x => x.Product)
                    .HasForeignKey(x => x.ProductId);
            });

            OnModelCreatingPartial(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}

