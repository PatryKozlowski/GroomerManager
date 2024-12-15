namespace GroomerManager.Web.Utils;

public class ErrorMapper
{
    public string ErrorToMessage(string? errorTitle)
    {
        return errorTitle switch
        {
            "AccountWithThisEmailAlreadyExists" => "Konto z tym adresem email już istnieje",
            "EmailIsRequired" => "Adres email jest wymagany",
            "InvalidEmailAddress" => "Podano nieprawidłowy adres email",
            "PasswordIsRequired" => "Hasło jest wymagane",
            "PasswordTooShort" => "Hasło jest zbyt krótkie. Powinno mieć co najmniej 8 znaków",
            "PasswordMustContainUppercase" => "Hasło musi zawierać co najmniej jedną wielką literę",
            "PasswordMustContainLowercase" => "Hasło musi zawierać co najmniej jedną małą literę",
            "PasswordMustContainNumber" => "Hasło musi zawierać co najmniej jedną cyfrę",
            "PasswordMustContainSpecialCharacter" => "Hasło musi zawierać co najmniej jeden znak specjalny",
            "RepeatPasswordIsRequired" => "Potwierdzenie hasła jest wymagane",
            "PasswordsDoNotMatch" => "Hasła nie są zgodne",
            "InvalidEmailOrPassword" => "Nieprawidłowy email lub hasło",
            "AccountLocked" => "Twoje konto zostało zablokowane. Skontaktuj się z administratorem",
            "ServerError" => "Wystąpił błąd serwera. Spróbuj ponownie później",
            "EmailNotConfirmed" => "Twój email nie został potwierdzony. Sprawdź skrzynkę pocztową",
            _ => "Wystąpił nieznany błąd"
        };
    }
}