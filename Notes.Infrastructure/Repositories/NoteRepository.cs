using Notes.Domain.Entities;
using Notes.Application.Interfaces;
using Notes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Notes.Infrastructure.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly NotesDbContext _dbContext;

    public NoteRepository(NotesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // En este caso, se utiliza para agregar una nueva nota a la base de datos de manera asincrona.
    public async Task AddAsync(Note note)
    {
        // guardar en la base de datos
        _dbContext.Notes.Add(note);
        await _dbContext.SaveChangesAsync();
    }

    // Representa una operacion que se ejecuta de forma asincrona y devuelve una lista de notas. En este caso, se utiliza para obtener todas las notas almacenadas en la base de datos de manera asincrona.
    public async Task<List<Note>> GetAllAsync()
    {
        return await _dbContext.Notes.ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        return await _dbContext.Notes.FindAsync(id);
    }

    public async Task<Note?> UpdateAsync(Note note)
    {
        _dbContext.Notes.Update(note);
        await _dbContext.SaveChangesAsync();
        return note;
    }

    public async Task DeleteAsync(int id)
    {
        var note = await _dbContext.Notes.FindAsync(id);
        if (note != null)
        {
            _dbContext.Notes.Remove(note);
            await _dbContext.SaveChangesAsync();
        }
    }
 
}