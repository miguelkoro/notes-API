using Notes.Application.Interfaces;

namespace Notes.Application.Notes;

public class DeleteNote
{
    private readonly INoteRepository _noteRepository;

    public DeleteNote(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<bool> ExecuteAsync(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        if (note == null)
        {
            return false;
        }
        await _noteRepository.DeleteAsync(id);
        return true;
    }
}