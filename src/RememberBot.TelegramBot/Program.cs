using Microsoft.EntityFrameworkCore;
using RememberBot.Kernel;
using RememberBot.Kernel.PipelineContext.Implementation;
using RememberBot.TelegramBot.DataBaseContext;
using RememberBot.TelegramBot.PipelineSteps;
using RememberBot.TelegramBot.PipelineSteps.AddTask;
using RememberBot.TelegramBot.PipelineSteps.ChangeDate;
using RememberBot.TelegramBot.PipelineSteps.ChangeLocalTime;
using RememberBot.TelegramBot.PipelineSteps.ChangeMessage;
using RememberBot.TelegramBot.PipelineSteps.ChangeTask;
using RememberBot.TelegramBot.PipelineSteps.None;
using RememberBot.TelegramBot.PipelineSteps.None.StartStep;
using RememberBot.TelegramBot.Services;
using RememberBot.TelegramBot.Workers;


 


var builder = Host.CreateApplicationBuilder(args);

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");


var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
optionsBuilder.UseSqlite(connection);
builder.Services.AddScoped<ApplicationContext>(db 
    => new ApplicationContext(optionsBuilder.Options));

builder.Configuration.AddJsonFile("token.json");
builder.Services.AddSingleton<DataBaseService>();
builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton(
   
    new PipelinesDistributor()
        .AddUnit(
            new Pipeline()
                .AddUnit(new StartCommand()), 
            TelegramState.All
            )
        .AddUnit(
            new Pipeline()
                .AddUnit(new ListTasksCommand())
                .AddUnit(new ChangeLocalTimeCommand())
                .AddUnit(new AddTaskCommand()),
            TelegramState.None)
        
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new ChangeLocalTimeCallback())
                .AddUnit(new ChangeLocalTimeStep()),
            TelegramState.ChangeLocalTime)
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new ChangeDateCallback())
                .AddUnit(new ChangeMessageCallback())
                .AddUnit(new AddTaskCallback()) 
                .AddUnit(new AddTaskStep()),
            TelegramState.AddTask) 
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new ChangeMessageStep())
                .AddUnit(new ChangePriority()),
            TelegramState.ChangeTask) 
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new ChangeDateStep()),
            TelegramState.ChangeDate)
        .AddUnit(
            new Pipeline()
                .AddUnit(new CancelButton())
                .AddUnit(new ChangeMessageStep()),
            TelegramState.ChangeMessage)
    );

builder.Services.AddHostedService<TelegramWorker>();
builder.Services.AddHostedService<CheckerUpcomingTasksWorker>();

var host = builder.Build();
host.Run();