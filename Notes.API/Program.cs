using Notes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

using Notes.Infrastructure.Repositories;
using Notes.Application.Interfaces;

using Notes.Application.Notes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Agrega el servicio de generacion de documentacion Swagger/OpenAPI al contenedor de servicios de la aplicacion. Esto permite que la aplicacion genere automaticamente una documentacion interactiva de la API, que puede ser utilizada para probar y explorar los endpoints disponibles.
builder.Services.AddControllers();

//Crea usando SQLite el NotesDbContext y la cadena de conexion NotesDatabase
builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("NotesDatabase")));

//Agrega la implementacion de INoteRepository, NoteRepository, al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<INoteRepository, NoteRepository>();

//Agrega la clase CreateNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<CreateNote>();
builder.Services.AddScoped<GetNotes>(); //Agrega la clase GetNotes al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<GetNote>(); //Agrega la clase GetNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<UpdateNote>(); //Agrega la clase UpdateNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<DeleteNote>(); //Agrega la clase DeleteNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Agrega el middleware de autorizacion al pipeline de procesamiento de solicitudes HTTP. Esto permite que la aplicacion verifique si el usuario que realiza la solicitud tiene los permisos necesarios para acceder a los recursos protegidos.
app.MapControllers();

/*var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
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
.WithName("GetWeatherForecast");
*/
app.Run();

/*record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
*/
