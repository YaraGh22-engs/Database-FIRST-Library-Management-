using Library_Management.IRepositories;
using Library_Management.Models;
using Library_Management.Repositories;
using Library_Management.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LibraryConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//service and repository registration
builder.Services.AddScoped<IBookeService, BookService>();
builder.Services.AddScoped<IBookReo, BookRepo>();
builder.Services.AddScoped<IAuthorRepo, AuthorRepo>();
builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
builder.Services.AddScoped<IPublisherRepo, PublisherRepo>();
builder.Services.AddScoped<ILIbraryRepo, LibraryRepo>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
