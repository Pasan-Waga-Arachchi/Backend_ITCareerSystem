﻿// <auto-generated />
using ITCareerSystem_Test1_.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ITCareerSystem_Test1_.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20240226130024_initial3")]
    partial class initial3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ITCareerSystem_Test1_.Models.User", b =>
                {
                    b.Property<string>("User_Name")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(255");

                    b.Property<string>("TP_Number")
                        .IsRequired()
                        .HasColumnType("varchar(15)");

                    b.HasKey("User_Name");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}
