namespace Notes.API.DTOs;

using System.ComponentModel.DataAnnotations;
using Notes.API.Errors;

public class UpdateNoteRequest
{
    [Required (ErrorMessage = ErrorCodes.TitleRequired)]
    [MaxLength(100, ErrorMessage = ErrorCodes.TitleTooLong)]
    public string? Title { get; set; }

    [MaxLength(1500, ErrorMessage = ErrorCodes.ContentTooLong)]
    public string? Content { get; set; }
}