using Starter.Api.Domain;

namespace Starter.Api.Features.Todos;

public static class TodoDeadlines
{
    public static bool IsOverdue(Todo todo, DateTime now)
    {
        return todo.DueDate != null
            && todo.DueDate < now
            && todo.IsDone == false;
    }
}