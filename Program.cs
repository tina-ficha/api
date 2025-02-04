using Microsoft.OpenApi.Models;
using TinaFicha.Application.Publish;
using TinaFicha.Ports.In.Publish;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    // generate http://localhost:{port}/swagger/v1/swagger.json
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TinaFicha", Version = "v1" });
});
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
builder.Services.AddControllers(); // reference de controllers
builder.Services.AddSingleton<PublishVideoOnPlatforms, PublishService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // see profiles launchSettings.json
{
    app.UseSwagger();
    app.UseSwaggerUI(); // generate http://localhost:{port}/swagger
}

app.MapControllers(); // Adds endpoints for controller
app.Run();

