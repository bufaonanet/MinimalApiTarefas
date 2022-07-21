using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("tarefas"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("", () => "Olá mundo!");

app.MapGet("frases", async () => await new HttpClient().GetStringAsync("https://ron-swanson-quotes.herokuapp.com/v2/quotes"));

app.MapGet("tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapGet("tarefas/{id}", async (AppDbContext db, int id) =>
{
    return (await db.Tarefas.FindAsync(id) is Tarefa tarefa)
        ? Results.Ok(tarefa)
        : Results.NotFound($"Nenhuma tarefa encontrado com Id {id}");
});

app.MapGet("tarefas/finalizadas", async (AppDbContext db) =>
{
    return await db.Tarefas.Where(t => t.Finalizada).ToListAsync();
});

app.MapPost("tarefas", async (AppDbContext db, Tarefa tarefa) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();
    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.MapPut("tarefas/{id}", async (AppDbContext db, Tarefa inputTarefa, int id) =>
{
    var tarefa = await db.Tarefas.FindAsync(id);
    if (tarefa == null) return Results.NotFound();

    tarefa.Nome = inputTarefa.Nome;
    tarefa.Finalizada = inputTarefa.Finalizada;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("tarefas/{id}", async (AppDbContext db, int id) =>
{
    if (await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        return Results.Ok(tarefa);
    }
    return Results.NotFound($"Nenhuma tarefa encontra com Id {id}");
});


app.Run();

#region Models
public class Tarefa
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public bool Finalizada { get; set; }
}
#endregion

#region Context
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();
}

#endregion