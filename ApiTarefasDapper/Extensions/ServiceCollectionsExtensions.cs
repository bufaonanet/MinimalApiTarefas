using System.Data.SqlClient;
using static ApiTarefasDapper.Data.TarefaContext;

namespace ApiTarefasDapper.Extensions;

public static class ServiceCollectionsExtensions
{
    public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

        builder.Services.AddScoped<GetConnection>(sp => async () =>
        {
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        });

        return builder;
    }
}
