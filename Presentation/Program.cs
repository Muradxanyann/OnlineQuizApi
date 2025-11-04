using OnlineQuizzApi;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

MigrationRunnerConfig.RunMigrations(connectionString!);

app.UseHttpsRedirection();




