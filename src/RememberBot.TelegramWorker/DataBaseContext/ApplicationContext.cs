using Microsoft.EntityFrameworkCore;
using RememberBot.TelegramWorker.DataBaseContext.Tables;

namespace RememberBot.TelegramWorker.DataBaseContext;

public sealed class ApplicationContext : DbContext {
    public DbSet<TelegramUser> Users { get; set; } = null!;
    public DbSet<Task> Tasks { get; set; } = null!;

    public ApplicationContext(DbContextOptions<DbContext> options):  base(options) {
        Database.EnsureCreated();
    }
}