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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () => // create route 'GET /weatherforecast'
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast"); // Name for OpenAPI (add endpoint Metadata)


app.MapControllers(); // Adds endpoints for controller

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


// Controlleur `GET health`
// return 200 => OK
