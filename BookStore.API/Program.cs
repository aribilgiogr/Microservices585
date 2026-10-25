using BookStore.API.Models;
using BookStore.API.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddSingleton<IBooksService, BookService>();

builder.Services.Configure<JsonOptions>(opt => opt.SerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();



app.UseHttpsRedirection();

var booksGroup = app.MapGroup("/api/books");

booksGroup.MapGet("/", async (IBooksService service) => Results.Ok(await service.GetAsync()));

booksGroup.MapGet("/{id:length(24)}", async (string id, IBooksService service) =>
{
    var book = await service.GetAsync(id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

booksGroup.MapPost("/", async (Book newBook, IBooksService service) =>
{
    await service.CreateAsync(newBook);
    return Results.Created($"/api/books/{newBook.Id}", newBook);
});

booksGroup.MapPut("/{id:length(24)}", async (string id, Book updatedBook, IBooksService service) =>
{
    var book = await service.GetAsync(id);

    if (book == null) return Results.NotFound();

    updatedBook.Id = book.Id;

    await service.UpdateAsync(id, updatedBook);

    return Results.NoContent();
});

booksGroup.MapDelete("/{id:length(24)}", async (string id, IBooksService service) =>
{
    var book = await service.GetAsync(id);
    if (book == null) return Results.NotFound();
    await service.RemoveAsync(id);
    return Results.NoContent();
});

app.Run();