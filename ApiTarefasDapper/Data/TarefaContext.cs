using System.Data;

namespace ApiTarefasDapper.Data;

public class TarefaContext
{
    public delegate Task<IDbConnection> GetConnection();
}
