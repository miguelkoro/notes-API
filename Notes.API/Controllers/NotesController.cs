using Microsoft.AspNetCore.Mvc;
using Notes.Application.Notes;
using Notes.API.DTOs;
using Notes.API.Errors;

namespace Notes.API.Controllers;

[ApiController] //Indica que esta clase es un controlador de API y que se encargará de manejar las solicitudes HTTP entrantes y devolver respuestas HTTP.
[Route("api/[controller]")] //Define la ruta base para las solicitudes HTTP que se dirigirán a este controlador. El [controller] se reemplazará automáticamente con el nombre del controlador, en este caso, "Notes".
public class NotesController : ControllerBase
{
    private readonly CreateNote _createNote;
    private readonly GetNotes _getNotes;
    private readonly GetNote _getNote;
    private readonly UpdateNote _updateNote;
    private readonly DeleteNote _deleteNote;
    public NotesController(CreateNote createNote, GetNotes getNotes, GetNote getNote, UpdateNote updateNote, DeleteNote deleteNote)
    {
        _createNote = createNote;
        _getNotes = getNotes;
        _getNote = getNote;
        _updateNote = updateNote;
        _deleteNote = deleteNote;
    }

    //Define un método de acción que maneja las solicitudes HTTP POST a la ruta "api/notes". Este método recibe un objeto CreateNoteRequest como parámetro, que contiene los datos necesarios para crear una nueva nota.
    [HttpPost]
    public async Task<IActionResult> Create(CreateNoteRequest request)
    { 
        var note = await _createNote.ExecuteAsync(
            request.Title,
            request.Content);

        //return Ok(note); //Devuelve una respuesta HTTP 200 OK con la nota creada en el cuerpo de la respuesta.
        return CreatedAtAction(//Devuelve una respuesta HTTP 201 Created con la nota creada en el cuerpo de la respuesta y la ubicación de la nueva nota en el encabezado Location.
            nameof(GetById), 
            new { id = note.Id }, 
            note);
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
            //return NotFound(); //Devuelve una respuesta HTTP 404 Not Found si la nota no existe.
            return NotFound(new ErrorResponse { //Devuelve una respuesta HTTP 404 Not Found si la nota no existe, con un mensaje personalizado.
                Errors = [ErrorCodes.NoteNotFound]
            });
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
            return NotFound(new ErrorResponse { //Devuelve una respuesta HTTP 404 Not Found si la nota no existe, con un mensaje personalizado.
                Errors = [ErrorCodes.NoteNotFound]
            });
        }

        return Ok(note);
    }

    //Define un método de acción que maneja las solicitudes HTTP DELETE a la ruta "api/notes/{id}". Este método elimina una nota específica por su ID y devuelve una respuesta HTTP 204 No Content si la eliminación fue exitosa.
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _deleteNote.ExecuteAsync(id);

        if (!deleted)
        {
            return NotFound(new ErrorResponse { //Devuelve una respuesta HTTP 404 Not Found si la nota no existe, con un mensaje personalizado.
                Errors = [ErrorCodes.NoteNotFound]
            });
        }

        await _deleteNote.ExecuteAsync(id);

        return NoContent(); //Devuelve una respuesta HTTP 204 No Content si la eliminación fue exitosa.
    }
}