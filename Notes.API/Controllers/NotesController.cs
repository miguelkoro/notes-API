using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes;
using Notes.API.DTOs;

namespace Notes.API.Controllers;

[ApiController] //Indica que esta clase es un controlador de API y que se encargará de manejar las solicitudes HTTP entrantes y devolver respuestas HTTP.
[Route("api/[controller]")] //Define la ruta base para las solicitudes HTTP que se dirigirán a este controlador. El [controller] se reemplazará automáticamente con el nombre del controlador, en este caso, "Notes".
public class NotesController : ControllerBase
{
    private readonly CreateNote _createNote;
    private readonly GetNotes _getNotes;
    private readonly GetNote _getNote;
    private readonly UpdateNote _updateNote;

    public NotesController(CreateNote createNote, GetNotes getNotes, GetNote getNote, UpdateNote updateNote)
    {
        _createNote = createNote;
        _getNotes = getNotes;
        _getNote = getNote;
        _updateNote = updateNote;

    }

    //Define un método de acción que maneja las solicitudes HTTP POST a la ruta "api/notes". Este método recibe un objeto CreateNoteRequest como parámetro, que contiene los datos necesarios para crear una nueva nota.
    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteRequest request)
    { 
        var note = await _createNote.ExecuteAsync(
            request.Title,
            request.Content);

        return Ok(note); //Devuelve una respuesta HTTP 200 OK con la nota creada en el cuerpo de la respuesta.
    }

    //Define un método de acción que maneja las solicitudes HTTP GET a la ruta "api/notes". Este método obtiene todas las notas almacenadas en la base de datos y las devuelve en una respuesta HTTP 200 OK.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var notes = await _getNotes.ExecuteAsync();

        return Ok(notes);
    }

    //Define un método de acción que maneja las solicitudes HTTP GET a la ruta "api/notes/{id}". Este método obtiene una nota específica por su ID y la devuelve en una respuesta HTTP 200 OK.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var note = await _getNote.ExecuteAsync(id);

        if (note == null)
        {
            return NotFound(); //Devuelve una respuesta HTTP 404 Not Found si la nota no existe.
        }

        return Ok(note);
    }

    //Define un método de acción que maneja las solicitudes HTTP PUT a la ruta "api/notes/{id}". Este método actualiza una nota específica por su ID y la devuelve en una respuesta HTTP 200 OK.
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateNoteRequest request)
    {
        var note = await _updateNote.ExecuteAsync(id, request.Title, request.Content);

        if (note == null)
        {
            return NotFound(); //Devuelve una respuesta HTTP 404 Not Found si la nota no existe.
        }

        return Ok(note);
    }
}