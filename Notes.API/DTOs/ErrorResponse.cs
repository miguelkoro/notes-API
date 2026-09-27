namespace Notes.API.DTOs;

public class ErrorResponse
{

    public List<string> Errors { get; set; } = []; //Propiedad para almacenar una lista de mensajes de error, inicializada como una lista vacía.

    //public string Code { get; set; } = string.Empty;

    //public string Message { get; set; } = string.Empty;

    //public Dictionary<string, string[]>? Errors { get; set; } //Propiedad opcional para almacenar errores de validación, donde la clave es el nombre del campo y el valor es un arreglo de mensajes de error asociados a ese campo.
}