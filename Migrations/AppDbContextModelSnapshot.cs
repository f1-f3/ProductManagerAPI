﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ProductManagerAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProductManagerAPI.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductID"));

                    b.Property<DateTime>("ProductAdded")
                        .HasColumnType("datetime2");

                    b.Property<int>("ProductGroupID")
                        .HasColumnType("int");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ProductPrice")
                        .HasColumnType("float");

                    b.Property<double>("ProductPriceWithVAT")
                        .HasColumnType("float");

                    b.Property<double>("ProductVAT")
                        .HasColumnType("float");

                    b.HasKey("ProductID");

                    b.HasIndex("ProductGroupID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.ProductGroup", b =>
                {
                    b.Property<int>("ProductGroupID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductGroupID"));

                    b.Property<int?>("MainGroupID")
                        .HasColumnType("int");

                    b.Property<string>("ProductGroupName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProductGroupID");

                    b.HasIndex("MainGroupID");

                    b.ToTable("ProductGroups");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.Store", b =>
                {
                    b.Property<int>("StoreID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StoreID"));

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StoreID");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.StoreProduct", b =>
                {
                    b.Property<int>("StoreID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.HasKey("StoreID", "ProductID");

                    b.HasIndex("ProductID");

                    b.ToTable("StoreProducts");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.Product", b =>
                {
                    b.HasOne("ProductManagerAPI.Models.ProductGroup", "ProductGroup")
                        .WithMany("Products")
                        .HasForeignKey("ProductGroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductGroup");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.ProductGroup", b =>
                {
                    b.HasOne("ProductManagerAPI.Models.ProductGroup", "MainGroup")
                        .WithMany("SubGroups")
                        .HasForeignKey("MainGroupID");

                    b.Navigation("MainGroup");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.StoreProduct", b =>
                {
                    b.HasOne("ProductManagerAPI.Models.Product", "Product")
                        .WithMany("StoreProducts")
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProductManagerAPI.Models.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.Product", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.ProductGroup", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("SubGroups");
                });

            modelBuilder.Entity("ProductManagerAPI.Models.Store", b =>
                {
                    b.Navigation("StoreProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
