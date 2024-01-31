using System.IO.Pipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.TelegramWorker;
using RememberBot.TelegramWorker.DataBaseContext;
using RememberBot.TelegramWorker.PipelineSteps;
using RememberBot.TelegramWorker.PipelineSteps.AddTask;
using RememberBot.TelegramWorker.PipelineSteps.ChangeLocalTime;
using RememberBot.TelegramWorker.PipelineSteps.None;
using RememberBot.TelegramWorker.Services;
using RememberBot.TelegramWorker.TelegramBotClient;


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);



var builder = Host.CreateApplicationBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");


var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseNpgsql(connection);
builder.Services.AddScoped<ApplicationContext>(db 
    => new ApplicationContext(optionsBuilder.Options));

builder.Configuration.AddJsonFile("token.json");
builder.Services.AddSingleton<DataBaseService>();
builder.Services.AddSingleton(
    new PipelinesDistributor()
        .AddUnit(
        new Pipeline()
            .AddUnit(new StartCommand())
            .AddUnit(new ChangeLocalTimeCommand())
            .AddUnit(new AddTaskCommand())
        ,
        TelegramState.None)
        .AddUnit(
        new Pipeline()
            .AddUnit(new CancelButton())
            .AddUnit(new ChangeLocalTimeCallback())
            .AddUnit(new ChangeLocalTimeStep())
        ,
        TelegramState.ChangeLocalTime)
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new AddTaskStep())
            ,
            TelegramState.AddTask)
    );

builder.Services.AddHostedService<TelegramWorker>();

var host = builder.Build();
host.Run();