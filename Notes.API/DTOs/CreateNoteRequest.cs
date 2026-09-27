namespace Notes.API.DTOs;
using System.ComponentModel.DataAnnotations;
using Notes.API.Errors;

public class CreateNoteRequest
{
    [Required (ErrorMessage = ErrorCodes.TitleRequired)] //Validacion de entrada para el campo Title, indicando que es obligatorio.
    [MaxLength(150, ErrorMessage = ErrorCodes.TitleTooLong)] //Validacion de entrada para el campo Title, indicando que no puede exceder los 150 caracteres.
    public string? Title { get; set; }

    [MaxLength(1500, ErrorMessage = ErrorCodes.ContentTooLong)]
    public string? Content { get; set; }
}