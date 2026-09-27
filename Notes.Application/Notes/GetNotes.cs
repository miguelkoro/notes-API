using Notes.Application.Interfaces;
using Notes.Domain.Entities;

namespace Notes.Application.Notes;

public class GetNotes
{
    private readonly INoteRepository _noteRepository;

    public GetNotes(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<List<Note>> ExecuteAsync()
    {
        return await _noteRepository.GetAllAsync();
    }
}