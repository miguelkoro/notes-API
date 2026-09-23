using Notes.Domain.Entities;

namespace Notes.Application.Interfaces;

public interface INoteRepository
{
    Task AddAsync(Note note); // Representa una operacion que se ejecuta de forma asincrona y no devuelve ningun valor, pero indica que la operacion se ha completado. En este caso, se utiliza para agregar una nueva nota a la base de datos de manera asincrona.
}