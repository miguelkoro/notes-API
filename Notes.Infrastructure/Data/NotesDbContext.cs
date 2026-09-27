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

    public DbSet<User> Users { get; set; } //Hace que user sea una entidad persistente en la base de datos y permite realizar operaciones CRUD sobre ella.

    // Configura el modelo de datos y aplica las configuraciones definidas en la clase NoteConfiguration.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(NotesDbContext).Assembly);*/
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id); // Define la propiedad Id como la clave primaria de la entidad Note.

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Content)
                .HasMaxLength(1500);

            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {

            entity.HasKey(e => e.Id); // Define la propiedad Id como la clave primaria de la entidad User.

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();
            
            entity.Property(e => e.PasswordHash)
                .IsRequired();

            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
        });
    }
}