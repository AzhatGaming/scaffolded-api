using Microsoft.EntityFrameworkCore;
using ScaffoldedApi;
using ScaffoldedApi.Swagger;
using WebApiExample.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.ScaffoldedApiQueryFilter();
});

builder.Services.AddApiScaffolding(opts =>
{
    opts.UseDbContext<SampleDbContext>();
});

builder.Services.AddDbContext<SampleDbContext>(options =>
{
    var connectionString = "Data source=test.db3";
    options.UseSqlite(connectionString);
});

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
