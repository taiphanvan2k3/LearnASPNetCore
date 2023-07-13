using Microsoft.EntityFrameworkCore;

namespace RazorWebTongHop.Models
{
    public class DataContext : DbContext
    {
        /// <summary>
        /// options sẽ được hệ thống DI inject vào khi dịch vụ DataContext được tạo ra
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            // thiết lập thêm....
        }

        public DbSet<Article> Articles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}