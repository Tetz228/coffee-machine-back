namespace WebApi.Db.Migrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(CoffeeMachineContext))]
    [Migration("20230121100417_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApi.Db.Models.Coffee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("MONEY")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.ToTable("Coffees");
                });

            modelBuilder.Entity("WebApi.Db.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<Guid>("FK_Orders_Coffees")
                        .HasColumnType("UUID");

                    b.Property<Guid?>("FK_Orders_Users")
                        .HasColumnType("UUID");

                    b.HasKey("Id");

                    b.HasIndex("FK_Orders_Coffees");

                    b.HasIndex("FK_Orders_Users");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApi.Db.Models.Statistic", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<Guid>("FK_Statistics_Coffees")
                        .HasColumnType("UUID");

                    b.Property<decimal>("Total")
                        .HasColumnType("MONEY")
                        .HasColumnName("total");

                    b.HasKey("Id");

                    b.HasIndex("FK_Statistics_Coffees");

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("WebApi.Db.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("UUID")
                        .HasColumnName("id");

                    b.Property<decimal>("Balance")
                        .HasColumnType("MONEY")
                        .HasColumnName("balance");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT")
                        .HasColumnName("password");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("TEXT")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("DATE")
                        .HasColumnName("refresh_token_expiry_time");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApi.Db.Models.Order", b =>
                {
                    b.HasOne("WebApi.Db.Models.Coffee", "Coffee")
                        .WithMany()
                        .HasForeignKey("FK_Orders_Coffees")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Db.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("FK_Orders_Users");

                    b.Navigation("Coffee");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebApi.Db.Models.Statistic", b =>
                {
                    b.HasOne("WebApi.Db.Models.Coffee", "Coffee")
                        .WithMany()
                        .HasForeignKey("FK_Statistics_Coffees")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coffee");
                });
        }
    }
}
