using WebLab1.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ProductService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
