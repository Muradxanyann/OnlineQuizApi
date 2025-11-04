using FluentMigrator.Runner;

namespace OnlineQuizzApi;

public static class MigrationRunnerConfig
{
    public static IServiceProvider CreateServices(string connectionString)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres() 
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(MigrationRunnerConfig).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }

    public static void RunMigrations(string connectionString)
    {
        var serviceProvider = CreateServices(connectionString);
        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }
}