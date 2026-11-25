namespace Starter.Api.Features.Todos;

/// <summary>What a client sends to create a todo.</summary>
public sealed record CreateTodoRequest(string Title, DateOnly? DueDate = null);

/// <summary>What a client gets back. This is the shape the frontend depends on.</summary>
public sealed record TodoResponse(int Id, string Title, bool IsDone, DateTimeOffset CreatedAt, DateOnly? DueDate);

public sealed record OverdueTodoResponse(
    int Id,
    string Title,
    DateOnly DueDate,
    int DaysLate);
