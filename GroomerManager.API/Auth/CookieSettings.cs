namespace GroomerManager.API.Auth;

public class CookieSettings
{
    public const string COOKIE_NAME = "auth.token";

    public bool Secure { get; set; } = true;

    public SameSiteMode SameSite { get; set; } = SameSiteMode.Lax;
}