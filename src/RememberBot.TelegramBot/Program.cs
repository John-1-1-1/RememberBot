using System.IO.Pipes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.TelegramBot;
using RememberBot.TelegramBot.DataBaseContext;
using RememberBot.TelegramBot.PipelineSteps;
using RememberBot.TelegramBot.PipelineSteps.AddTask;
using RememberBot.TelegramBot.PipelineSteps.ChangeLocalTime;
using RememberBot.TelegramBot.PipelineSteps.None;
using RememberBot.TelegramBot.PipelineSteps.None.StartStep;
using RememberBot.TelegramBot.Services;
using RememberBot.TelegramBot.Workers;


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
builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton(
   
    new PipelinesDistributor()
        .AddUnit(
        new Pipeline()
            .AddUnit(new StartCommand())
            .AddUnit(new ListTasksCommand())
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
                .AddUnit(new AddTaskCallback())
                .AddUnit(new AddTaskStep())
            ,
            TelegramState.AddTask)
    );

builder.Services.AddHostedService<TelegramWorker>();
builder.Services.AddHostedService<CheckerUpcomingTasksWorker>();

var host = builder.Build();
host.Run();