using Starter.Api.Domain;

namespace Starter.Api.Features.Todos;

public static class TodoDeadlines
{
    public static bool IsOverdue(Todo todo, DateOnly today)
    {
        return todo.DueDate.HasValue
            && todo.DueDate.Value < today
            && !todo.IsDone;
    }
}
