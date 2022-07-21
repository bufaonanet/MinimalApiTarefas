using ApiTarefasDapper.Endpoints;
using ApiTarefasDapper.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPersistence();

var app = builder.Build();
app.MapTarefasEndpoints();

app.Run();