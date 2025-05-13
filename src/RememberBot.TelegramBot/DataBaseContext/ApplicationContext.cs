using Microsoft.EntityFrameworkCore;
using RememberBot.Kernel.Tables;

namespace RememberBot.TelegramBot.DataBaseContext;

public sealed class ApplicationContext : DbContext {
    public DbSet<TelegramUser> Users { get; set; } = null!;
    public DbSet<TelegramTask> Task { get; set; } = null!;

    public ApplicationContext(DbContextOptions<DbContext> options):  base(options) { 
        Database.EnsureCreated();
    }
}