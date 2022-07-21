﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiTarefasDapper.Data;

[Table("Tarefas")]
public record Tarefa(int Id, string Atividade, string Status);