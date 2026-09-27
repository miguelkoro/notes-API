using Notes.Application.Interfaces;
using Notes.Domain.Entities;

namespace Notes.Application.Notes;

public class CreateNote
{
    private readonly INoteRepository _noteRepository;

    public CreateNote(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Note> ExecuteAsync(string title, string content)
    {
        var note = new Note(title, content);

        await _noteRepository.AddAsync(note);

        return note; //Hacemos que devuelva la nota creada para poder usarla en el controlador y devolverla en la respuesta HTTP.
    }
}