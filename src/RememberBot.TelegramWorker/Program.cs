using Microsoft.EntityFrameworkCore;
using RememberBot.TelegramWorker;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.TelegramBotClient;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);



var builder = Host.CreateApplicationBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Configuration.AddJsonFile("token.json");
builder.Services.AddSingleton<TelegramClient>();
builder.Services.AddHostedService<Worker>();

var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseNpgsql(connection);
builder.Services.AddScoped<ApplicationContext>(db 
    => new ApplicationContext(optionsBuilder.Options));

var host = builder.Build();
host.Run();