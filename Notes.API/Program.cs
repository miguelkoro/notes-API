using Notes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Notes.Infrastructure.Repositories;
using Notes.Application.Interfaces;

using Notes.Application.Notes;
using Notes.Application.Auth;
using Notes.Infrastructure.Security;
using Notes.API.Errors;
using Notes.API.DTOs;

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

//NOTAS
//Agrega la clase CreateNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<CreateNote>();
builder.Services.AddScoped<GetNotes>(); //Agrega la clase GetNotes al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<GetNote>(); //Agrega la clase GetNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<UpdateNote>(); //Agrega la clase UpdateNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<DeleteNote>(); //Agrega la clase DeleteNote al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)

//USER
builder.Services.AddScoped<IUserRepository, UserRepository>(); //Agrega la implementacion de IUserRepository, UserRepository, al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

builder.Services.AddScoped<RegisterUser>();

//Agrega el servicio de manejo de excepciones globales al contenedor de servicios de la aplicacion. Esto permite que la aplicacion capture y maneje las excepciones no controladas que ocurren durante el procesamiento de las solicitudes HTTP, y devuelva respuestas HTTP adecuadas con informacion sobre el error.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .SelectMany(x => x.Value!.Errors)
            .Select(x => x.ErrorMessage)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        return new BadRequestObjectResult(new ErrorResponse
        {
            Errors = errors
        });
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Agrega el middleware de autorizacion al pipeline de procesamiento de solicitudes HTTP. Esto permite que la aplicacion verifique si el usuario que realiza la solicitud tiene los permisos necesarios para acceder a los recursos protegidos.
app.MapControllers();


app.Run();

