using Notes.Application.Interfaces;
using Notes.Domain.Entities;

namespace Notes.Application.Notes;

public class UpdateNote
{
    private readonly INoteRepository _noteRepository;

    public UpdateNote(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Note?> ExecuteAsync(int id, string? title, string? content)
    {
        var note = await _noteRepository.GetByIdAsync(id);

        if (note == null)
        {
            return null;
        }

        note.Update(title, content);

        await _noteRepository.UpdateAsync(note);

        return note; //Hacemos que devuelva la nota creada para poder usarla en el controlador y devolverla en la respuesta HTTP.
    }
}