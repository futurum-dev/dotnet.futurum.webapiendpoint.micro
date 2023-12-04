using Microsoft.Data.Sqlite;

namespace Futurum.WebApiEndpoint.Micro.Sample.Todo;

[WebApiEndpoint("api/todos", "todos")]
public partial class TodoApiWebApiEndpoint
{
    protected override void Build(IEndpointRouteBuilder builder)
    {
        builder.MapGet("/", GetAllHandler)
               .WithName("GetAllTodos");

        builder.MapGet("/complete", GetCompleteHandler)
               .WithName("GetCompleteTodos");

        builder.MapGet("/incomplete", GetIncompleteHandler)
               .WithName("GetIncompleteTodos");

        builder.MapGet("/{id:int}", GetByIdHandler)
               .WithName("GetTodoById");

        builder.MapGet("/find", FindHandler)
               .WithName("FindTodo");

        builder.MapPost("/", CreateHandler)
               .WithName("CreateTodo");

        builder.MapPut("/{id}", UpdateHandler)
               .WithName("UpdateTodo");

        builder.MapPut("/{id}/mark-complete", MarkCompleteHandler)
               .WithName("MarkComplete");

        builder.MapPut("/{id}/mark-incomplete", MarkIncompleteHandler)
               .WithName("MarkIncomplete");

        builder.MapDelete("/{id}", DeleteHandler)
               .WithName("DeleteTodo");

        builder.MapDelete("/delete-all", DeleteAllHandler)
               .WithName("DeleteAll");
    }

    private static Results<Ok<IAsyncEnumerable<Todo>>, BadRequest<ProblemDetails>> GetAllHandler(HttpContext context, SqliteConnection db)
    {
        return RunToOk(Execute, context, "Failed to get todos");

        IAsyncEnumerable<Todo> Execute() =>
            db.QueryAsync<Todo>("SELECT * FROM Todos");
    }

    private static Results<Ok<IAsyncEnumerable<Todo>>, BadRequest<ProblemDetails>> GetCompleteHandler(HttpContext context, SqliteConnection db)
    {
        return RunToOk(Execute, context, "Failed to get complete todos");

        IAsyncEnumerable<Todo> Execute() =>
            db.QueryAsync<Todo>("SELECT * FROM Todos WHERE IsComplete = true");
    }

    private static Results<Ok<IAsyncEnumerable<Todo>>, BadRequest<ProblemDetails>> GetIncompleteHandler(HttpContext context, SqliteConnection db)
    {
        return RunToOk(Execute, context, "Failed to get incomplete todos");

        IAsyncEnumerable<Todo> Execute() =>
            db.QueryAsync<Todo>("SELECT * FROM Todos WHERE IsComplete = false");
    }

    private static Task<Results<Ok<Todo>, NotFound, BadRequest<ProblemDetails>>> GetByIdHandler(HttpContext context, SqliteConnection db, int id)
    {
        return RunAsync(Execute, context, () => $"Failed to find todo with id '{id}'");

        async Task<Results<Ok<Todo>, NotFound>> Execute() =>
            await db.QuerySingleAsync<Todo>("SELECT * FROM Todos WHERE Id = @id", id.AsDbParameter()) is Todo todo
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound();
    }

    private static Task<Results<Ok<Todo>, NoContent, BadRequest<ProblemDetails>>> FindHandler(HttpContext context, SqliteConnection db, string title, bool? isComplete)
    {
        return RunAsync(Execute, context, () => $"Failed to find todo with title '{title}'");

        async Task<Results<Ok<Todo>, NoContent>> Execute() =>
            await db.QuerySingleAsync<Todo>("SELECT * FROM Todos WHERE Title = @title COLLATE NOCASE AND (@isComplete is NULL OR IsComplete = @isComplete)",
                                            title.AsDbParameter(),
                                            isComplete.AsDbParameter()) is Todo todo
                ? TypedResults.Ok(todo)
                : TypedResults.NoContent();
    }

    private static Task<Results<Created<Todo>, BadRequest<ProblemDetails>>> CreateHandler(HttpContext context, SqliteConnection db, Todo todo)
    {
        return RunAsync(Execute, context, () => "Failed to create todo");

        async Task<Created<Todo>> Execute()
        {
            var createdTodo = await db.QuerySingleAsync<Todo>("INSERT INTO Todos(Title, IsComplete) Values(@Title, @IsComplete) RETURNING *",
                                                              todo.Title.AsDbParameter(),
                                                              todo.IsComplete.AsDbParameter());

            return TypedResults.Created($"/todos/{createdTodo?.Id}", createdTodo);
        }
    }

    private static Task<Results<NoContent, NotFound, BadRequest<ProblemDetails>>> UpdateHandler(HttpContext context, SqliteConnection db, int id, Todo inputTodo)
    {
        inputTodo.Id = id;

        return RunAsync(Execute, context, () => "Failed to update todo");

        async Task<Results<NoContent, NotFound>> Execute() =>
            await db.ExecuteAsync("UPDATE Todos SET Title = @Title, IsComplete = @IsComplete WHERE Id = @Id", inputTodo.Title.AsDbParameter(), inputTodo.IsComplete.AsDbParameter()) == 1
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
    }

    private static Task<Results<NoContent, NotFound, BadRequest<ProblemDetails>>> MarkCompleteHandler(HttpContext context, SqliteConnection db, int id)
    {
        return RunAsync(Execute, context, () => $"Failed to mark todo with id '{id}' as incomplete");

        async Task<Results<NoContent, NotFound>> Execute() =>
            await db.ExecuteAsync("UPDATE Todos SET IsComplete = true WHERE Id = @id", id.AsDbParameter()) == 1
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
    }

    private static Task<Results<NoContent, NotFound, BadRequest<ProblemDetails>>> MarkIncompleteHandler(HttpContext context, SqliteConnection db, int id)
    {
        return RunAsync(Execute, context, () => $"Failed to mark todo with id '{id}' as incomplete");

        async Task<Results<NoContent, NotFound>> Execute() =>
            await db.ExecuteAsync("UPDATE Todos SET IsComplete = false WHERE Id = @id", id.AsDbParameter()) == 1
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
    }

    private static Task<Results<NoContent, NotFound, BadRequest<ProblemDetails>>> DeleteHandler(HttpContext context, SqliteConnection db, int id)
    {
        return RunAsync(Execute, context, () => $"Failed to delete todo with id '{id}'");

        async Task<Results<NoContent, NotFound>> Execute() =>
            await db.ExecuteAsync("DELETE FROM Todos WHERE Id = @id", id.AsDbParameter()) == 1
                ? TypedResults.NoContent()
                : TypedResults.NotFound();
    }

    private static Task<Results<Ok<int>, BadRequest<ProblemDetails>>> DeleteAllHandler(HttpContext context, SqliteConnection db)
    {
        return RunToOkAsync(Execute, context, "Failed to delete all todos");

        Task<int> Execute() =>
            db.ExecuteAsync("DELETE FROM Todos");
    }
}
