using Microsoft.EntityFrameworkCore;
using Notes.Domain.Entities;

namespace Notes.Infrastructure.Data;

public class NotesDbContext : DbContext
{
    public NotesDbContext(DbContextOptions<NotesDbContext> options) //Constructor que recibe las opciones de configuración para la base de datos y las pasa al constructor base de DbContext.
        : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; } //Hace que note sea una entidad persistente en la base de datos y permite realizar operaciones CRUD sobre ella.

    // Configura el modelo de datos y aplica las configuraciones definidas en la clase NoteConfiguration.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NotesDbContext).Assembly);
    }
}