namespace Notes.API.Errors;

public static class ErrorCodes
{
    public const string TitleRequired = "TITLE_REQUIRED"; //Titulo obligatorio

    public const string TitleTooLong = "TITLE_TOO_LONG"; //Titulo demasiado largo

    public const string ContentTooLong = "CONTENT_TOO_LONG"; //Contenido demasiado largo

    public const string NoteNotFound = "NOTE_NOT_FOUND"; //Nota no encontrada
}