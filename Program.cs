var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // reference de controllers

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // see profiles launchSettings.json
{
    app.MapOpenApi(); // generate http://localhost:{port}/openapi/v1.json
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // automatically redirect http to https 

app.MapControllers(); // Adds endpoints for controller

app.Run();

