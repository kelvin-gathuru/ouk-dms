using System;
using System.Threading.Tasks;
using Dapper;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DocumentManagement.API.Helpers;

public class HangfireCleanupService
{
    private readonly string _connString;
    private readonly ILogger<HangfireCleanupService> _logger;

    public HangfireCleanupService(IConfiguration configuration, ILogger<HangfireCleanupService> logger)
    {
        _connString = configuration.GetConnectionString("DocumentDbConnectionString");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [Queue("cleanup")]
    [AutomaticRetry(Attempts = 3, DelaysInSeconds = new[] { 60, 300, 900 })]
    [DisableConcurrentExecution(3600)]
    public async Task CleanupOldJobs()
    {
        try
        {
            using var conn = new MySqlConnection(_connString);
            await conn.OpenAsync();

            // Delete succeeded jobs older than 1 days
            var sql = @"
            DELETE FROM hangfirejob WHERE StateName = 'Succeeded' AND CreatedAt < DATE_SUB(NOW(), INTERVAL 1 DAY);
            DELETE FROM hangfirejobparameter WHERE JobId NOT IN (SELECT Id FROM hangfirejob);
            DELETE FROM hangfirejobstate WHERE JobId NOT IN (SELECT Id FROM hangfirejob);
            DELETE FROM hangfirehash WHERE ExpireAt IS NOT NULL AND ExpireAt < @now;
            DELETE FROM hangfireset WHERE ExpireAt IS NOT NULL AND ExpireAt < @now ";

            await conn.ExecuteAsync(sql);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during Hangfire cleanup");
        }
    }
}
