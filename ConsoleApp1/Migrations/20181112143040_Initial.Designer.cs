﻿// <auto-generated />
using ConsoleApp1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp1.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20181112143040_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.0-preview3-35497")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ConsoleApp1.Role", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .IsUnicode(true);

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("UX_Role_Name")
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
