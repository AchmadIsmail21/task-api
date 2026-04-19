using Microsoft.EntityFrameworkCore;
using TaskApi_belajar.Models;

namespace TaskApi_belajar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<TaskItem> TaskItems { get; set; }   
        public DbSet<User> Users { get; set; }
    }
}
