using Microsoft.EntityFrameworkCore;

namespace Many_To_Many_API_Demo.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookOrder>()
                .HasKey(bo => new { bo.BookId, bo.OrderId }); // ✅ Composite Primary Key

            modelBuilder.Entity<BookOrder>()
                .HasOne(bo => bo.Book)
                .WithMany(b => b.bookOrders)
                .HasForeignKey(bo => bo.BookId);

            modelBuilder.Entity<BookOrder>()
                .HasOne(bo => bo.Order)
                .WithMany(o => o.bookOrders)
                .HasForeignKey(bo => bo.OrderId);
        }
    }
}
