using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using IDbCommandInterceptor = Microsoft.EntityFrameworkCore.Diagnostics.IDbCommandInterceptor;

namespace ClientUtilsProject.Utils;

public class MyCommandInterceptor : IDbCommandInterceptor
{
    private static ISportLogger Logger { get; set; }
    
    public static void Log(string comm, string message) {
        Logger.Information("Intercepted: {comm}, Command Text: {message} ");
    }

    public void NonQueryExecuted(DbCommand command, 
        DbCommandInterceptionContext<int> interceptionContext) {
        Log("NonQueryExecuted: ", command.CommandText);
    }

    public void NonQueryExecuting(DbCommand command, 
        DbCommandInterceptionContext<int> interceptionContext) {
        Log("NonQueryExecuting: ", command.CommandText);
    }

    public void ReaderExecuted(DbCommand command, 
        DbCommandInterceptionContext<DbDataReader> interceptionContext) {
        Log("ReaderExecuted: ", command.CommandText);
    }

    public void ReaderExecuting(DbCommand command, 
        DbCommandInterceptionContext<DbDataReader> interceptionContext) {
        Log("ReaderExecuting: ", command.CommandText);
    }

    public void ScalarExecuted(DbCommand command, 
        DbCommandInterceptionContext<object> interceptionContext) {
        Log("ScalarExecuted: ", command.CommandText);
    }

    public void ScalarExecuting(DbCommand command, 
        DbCommandInterceptionContext<object> interceptionContext) {
        Log("ScalarExecuting: ", command.CommandText);
    }

    public MyCommandInterceptor(ISportLogger logger)
    {
        Logger = logger;
    }
}