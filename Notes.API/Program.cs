using Notes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Notes.Infrastructure.Repositories;
using Notes.Application.Interfaces;
using Notes.Application.Auth;
using Notes.Application.Notes;
using Notes.Application.Errors;
using Notes.Infrastructure.Security;
using Notes.API.DTOs;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


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
builder.Services.AddScoped<LoginUser>();

builder.Services.AddScoped<ITokenService, JwtTokenService>(); //Agrega la implementacion de ITokenService, JwtTokenService, al contenedor de servicios de la aplicacion. (Scopped = crea instancia por peticion http)

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

//Agrega la configuracion de JWT al contenedor de servicios de la aplicacion. Esto permite que la aplicacion lea la configuracion de JWT desde el archivo de configuracion (appsettings.json) y la utilice para generar y validar tokens JWT en las solicitudes HTTP.
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

//Agrega el servicio de autenticacion y configuracion de JWT al contenedor de servicios de la aplicacion. Esto permite que la aplicacion autentique a los usuarios mediante tokens JWT, y valide los tokens en las solicitudes HTTP entrantes.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("Jwt")
            .Get<JwtSettings>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,

            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });

var app = builder.Build();

// Define un endpoint para el registro de usuarios en la ruta "/api/auth/register". Este endpoint maneja las solicitudes HTTP POST y recibe un objeto RegisterRequest como parámetro, que contiene los datos necesarios para registrar un nuevo usuario. El endpoint utiliza la clase RegisterUser para procesar la solicitud y devolver una respuesta HTTP 201 Created con la información del usuario registrado en el cuerpo de la respuesta.
app.MapPost("/api/auth/register", async (
    RegisterRequest request,
    RegisterUser registerUser) =>
{
    var result = await registerUser.ExecuteAsync(request);

    if (!result.Success)
    {
        return result.ErrorCode switch
        {
            ApplicationErrorCodes.InvalidEmail =>
                Results.BadRequest(new ErrorResponse
                {
                    Errors = [result.ErrorCode]
                }),

            ApplicationErrorCodes.InvalidPassword =>
                Results.BadRequest(new ErrorResponse
                {
                    Errors = [result.ErrorCode]
                }),

            ApplicationErrorCodes.EmailAlreadyExists =>
                Results.Conflict(new ErrorResponse
                {
                    Errors = [result.ErrorCode]
                }),

            _ =>
                Results.BadRequest(new ErrorResponse
                {
                    Errors = [result.ErrorCode ?? "UNKNOWN_ERROR"]
                })
        };
    }

    return Results.Created(
        $"/api/users/{result.Id}",
        result);
});

app.MapPost("/api/auth/login", async (
    LoginRequest request,
    LoginUser loginUser) =>
{
    var result = await loginUser.ExecuteAsync(request);

    if (!result.Success)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(result);
});

// Define un endpoint para obtener la información del usuario autenticado en la ruta "/api/auth/me". Este endpoint maneja las solicitudes HTTP GET y utiliza el contexto de la solicitud (HttpContext) para acceder a la información del usuario autenticado. Devuelve una respuesta HTTP 200 OK con un objeto que contiene el estado de autenticación, el ID del usuario, el correo electrónico y el rol del usuario.
app.MapGet("/api/auth/me", (HttpContext context) =>
{
    return Results.Ok(new
    {
        authenticated = context.User.Identity?.IsAuthenticated,

        userId = context.User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,

        email = context.User.FindFirst(
            System.Security.Claims.ClaimTypes.Email)?.Value,

        role = context.User.FindFirst(
            System.Security.Claims.ClaimTypes.Role)?.Value
    });
})
.RequireAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); //Agrega el middleware de redireccionamiento HTTPS al pipeline de procesamiento de solicitudes HTTP. Esto permite que la aplicacion redirija automaticamente las solicitudes HTTP entrantes a HTTPS, mejorando la seguridad de la comunicacion entre el cliente y el servidor.

//Agrega el middleware de manejo de excepciones globales al pipeline de procesamiento de solicitudes HTTP. Esto permite que la aplicacion capture y maneje las excepciones no controladas que ocurren durante el procesamiento de las solicitudes HTTP, y devuelva respuestas HTTP adecuadas con informacion sobre el error.
app.UseAuthentication();
app.UseAuthorization();

//Agrega el middleware de autorizacion al pipeline de procesamiento de solicitudes HTTP. Esto permite que la aplicacion verifique si el usuario que realiza la solicitud tiene los permisos necesarios para acceder a los recursos protegidos.
app.MapControllers();


app.Run();

