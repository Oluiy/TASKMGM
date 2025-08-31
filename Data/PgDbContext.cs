using Microsoft.EntityFrameworkCore;
using TaskManager.Model;

namespace program.Data
{
    public class PgDbContext : DbContext
    {
        public PgDbContext(DbContextOptions<PgDbContext> options): base(options) {}
        public DbSet<TaskModel> Tasks { get; set; }
    }
}

