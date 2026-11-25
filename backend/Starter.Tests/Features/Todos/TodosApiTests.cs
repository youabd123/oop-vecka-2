using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Starter.Api.Features.Todos;

namespace Starter.Tests.Features.Todos;

/// <summary>Starts the real API in memory, with its own throwaway SQLite file, and talks to it over HTTP.</summary>
public sealed class TodosApiTests : IClassFixture<TodosApiTests.ApiFactory>
{
    private readonly HttpClient _client;

    public TodosApiTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_then_get_returns_the_created_todo()
    {
        var created = await _client.PostAsJsonAsync("/api/todos", new CreateTodoRequest("Write a test"));
        Assert.Equal(HttpStatusCode.Created, created.StatusCode);

        var todos = await _client.GetFromJsonAsync<List<TodoResponse>>("/api/todos");

        Assert.Contains(todos!, t => t.Title == "Write a test");
    }

    [Fact]
    public async Task Post_with_empty_title_returns_400()
    {
        var response = await _client.PostAsJsonAsync("/api/todos", new CreateTodoRequest(""));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(22, 59, false)]
    [InlineData(23, 0, true)]
    [Trait("Krav", "AK-2.1")]
    [Trait("Krav", "AK-2.2")]
    public async Task Overdue_uses_the_swedish_calendar_day(
        int utcHour,
        int utcMinute,
        bool expectedOverdue)
    {
        var clock = new FakeTimeProvider(
            new DateTimeOffset(
                2026, 11, 24, utcHour, utcMinute, 0, TimeSpan.Zero));

        using var factory = new ApiFactory();
        using var configuredFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<TimeProvider>(clock);
            });
        });
        using var client = configuredFactory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/todos",
            new CreateTodoRequest(
                "Skicka offert",
                new DateOnly(2026, 11, 24)));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<TodoResponse>();
        Assert.NotNull(created);

        var overdue = await client.GetFromJsonAsync<List<OverdueTodoResponse>>(
            "/api/todos/overdue");

        Assert.NotNull(overdue);
        Assert.Equal(
            expectedOverdue,
            overdue.Any(todo => todo.Id == created.Id));

        if (expectedOverdue)
        {
            var todo = Assert.Single(overdue, todo => todo.Id == created.Id);
            Assert.Equal(1, todo.DaysLate);
        }
    }

    public sealed class ApiFactory : WebApplicationFactory<Program>
    {
        private readonly string _databasePath = Path.Combine(Path.GetTempPath(), $"starter-test-{Guid.NewGuid()}.db");

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder.UseSetting("ConnectionStrings:Default", $"Data Source={_databasePath}");

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();
            File.Delete(_databasePath);
        }
    }
}
