using Notes.Domain.Entities;
using Notes.Application.Interfaces;

namespace Notes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    public async Task AddAsync(Note note)
    {
        // guardar en la base de datos
    }
}