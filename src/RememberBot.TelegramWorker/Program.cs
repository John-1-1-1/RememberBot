using System.IO.Pipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.TelegramWorker;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.PipelineSteps;
using RememberBot.TelegramWorker.Services;
using RememberBot.TelegramWorker.TelegramBotClient;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);



var builder = Host.CreateApplicationBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Configuration.AddJsonFile("token.json");
builder.Services.AddSingleton<TelegramClient>();
builder.Services.AddSingleton<DataBaseService>();
builder.Services.AddSingleton<PipelinesDistributor>(
    new PipelinesDistributor().AddUnit(
        new Pipeline().AddUnit(new StartCommand())
        , TelegramState.None)
    );

builder.Services.AddHostedService<Worker>();

var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseNpgsql(connection);
builder.Services.AddScoped<ApplicationContext>(db 
    => new ApplicationContext(optionsBuilder.Options));

var host = builder.Build();
host.Run();