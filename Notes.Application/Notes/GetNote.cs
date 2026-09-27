using Notes.Application.Interfaces;
using Notes.Domain.Entities;

namespace Notes.Application.Notes;

public class GetNote
{
    private readonly INoteRepository _noteRepository;

    public GetNote(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public async Task<Note?> ExecuteAsync(int id)
    {
        return await _noteRepository.GetByIdAsync(id);
    }
}