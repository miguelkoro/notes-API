namespace Notes.Domain.Entities;

//Creo la clase Note que representa una nota en el dominio de la aplicación.
//El titulo sera obligatorio con un maximo de 150 caracteres
//El contenido no sera obligatorio y tendra un maximo de 1500 caracteres
public class Note
{
    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty; //= null!;

    public string Content { get; private set; } = string.Empty; //= null!;

    public DateTime CreatedAt { get; private set; }

    // Creo dos constructores, uno vacío y otro que reciba los parámetros necesarios para crear una nota.
    private Note()
    {  
    }

    //Este constructor es el que se utilizará para crear una nueva nota, 
    // asegurando que se cumplan las validaciones establecidas para el título y el contenido. 
    public Note(string title, string content)
    {

        //Añado validaciones para asegurar que el título y el contenido cumplan con los requisitos establecidos.
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("El titulo es obligatorio.", nameof(title));
        }

        if (title.Length > 150)
        {
            throw new ArgumentException("El titulo no puede exceder los 150 caracteres.", nameof(title));
        }

        if (content != null && content.Length > 1500)
        {
            throw new ArgumentException("El contenido no puede exceder los 1500 caracteres.", nameof(content));
        }

        Title = title;
        Content = content;
        CreatedAt = DateTime.UtcNow; // Asigno la fecha y hora actual en formato UTC al crear una nueva nota.
    }
}