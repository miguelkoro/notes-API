using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain.Entities;

namespace Notes.Infrastructure.Configurations;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("Notes");

        builder.HasKey(note => note.Id);

        builder.Property(note => note.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(note => note.Content)
            .IsRequired(false)
            .HasMaxLength(1500);

        builder.Property(note => note.CreatedAt)
            .IsRequired();
    }
}