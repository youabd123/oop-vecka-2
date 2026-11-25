using Starter.Api.Domain;
using Starter.Api.Features.Todos;
using Xunit;

namespace Starter.Tests.Features.Todos;

public class TodoDeadlinesTests
{
    [Fact]
    [Trait("Krav", "AK-2.1")]
    public void A_todo_due_today_is_not_overdue()
    {
        var todo = new Todo
        {
            Title = "Skicka offert",
            DueDate = new DateOnly(2026, 11, 24),
            IsDone = false
        };

        var now = new DateTime(2026, 11, 24, 23, 59, 0);

        var result = TodoDeadlines.IsOverdue(todo, DateOnly.FromDateTime(now));

        Assert.False(result);
    }
}
