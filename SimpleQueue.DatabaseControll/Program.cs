using Hangfire;
using Hangfire.MySql;
using SimpleQueue.Data;
using SimpleQueue.DatabaseControll.Jobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("mySqlHangfireDbConnection");

builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseStorage(
            new MySqlStorage(
                connectionString,
                new MySqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablesPrefix = "Hangfire"
                })));

builder.Services.AddScoped<IDatabaseManager, DatabaseManager>();

builder.Services.AddDbContext<SimpleQueueDBContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("mySqlConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddHangfireServer();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseHangfireDashboard();
app.MapHangfireDashboard();

RecurringJob.AddOrUpdate<IDatabaseManager>(job => job.SendMessage(), "0 * * ? * *");
RecurringJob.AddOrUpdate<IDatabaseManager>(job => job.CheckUserInQueueTable(), "*/10 * * * *");

app.Run();