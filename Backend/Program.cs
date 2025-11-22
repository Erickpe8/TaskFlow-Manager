var builder = WebApplication.CreateBuilder(args);

// OpenAPI básico (solo JSON, SIN UI)
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // esto sí funciona → /openapi/v1.json
}

// app.UseHttpsRedirection();  // déjalo apagado para no dar warnings

app.MapGet("/weatherforecast", () =>
{
    return Enumerable.Range(1, 5).Select(index =>
        new
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = "OK"
        });
});

app.Run();
