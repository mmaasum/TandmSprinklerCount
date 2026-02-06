using TandmSprinklerCount.Data;
using TandmSprinklerCount.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<IFireDesignRepository, FireDesignRepository>();
builder.Services.AddScoped<ISprinklerService, SprinklerService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => Results.Redirect("/FireDesign"));

app.Run();
