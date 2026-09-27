using Notes.Domain.Entities;


namespace Notes.Application.Interfaces;

public interface INoteRepository
{
    //POST: /api/notes
    Task AddAsync(Note note); // Representa una operacion que se ejecuta de forma asincrona y no devuelve ningun valor, pero indica que la operacion se ha completado. En este caso, se utiliza para agregar una nueva nota a la base de datos de manera asincrona.

    Task<List<Note>> GetAllAsync(); // Representa una operacion que se ejecuta de forma asincrona y devuelve una lista de notas. En este caso, se utiliza para obtener todas las notas almacenadas en la base de datos de manera asincrona.

    Task<Note?> GetByIdAsync(int id); // Representa una operacion que se ejecuta de forma asincrona y devuelve una nota. En este caso, se utiliza para obtener una nota almacenada en la base de datos de manera asincrona.

    Task<Note?> UpdateAsync(Note note); // Representa una operacion que se ejecuta de forma asincrona y no devuelve ningun valor, pero indica que la operacion se ha completado. En este caso, se utiliza para actualizar una nota existente en la base de datos de manera asincrona.
}