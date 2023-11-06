using Microsoft.EntityFrameworkCore;
using Minio_Demo.Persistence;
using Minio_Demo.Services.Abstractions;
using Minio_Demo.Services.Implementations;
using Minio_Demo.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.Configure<MinioSettings>(builder.Configuration.GetSection(nameof(MinioSettings)));
builder.Services.AddScoped<IFileManager, MinioFileManager>();

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