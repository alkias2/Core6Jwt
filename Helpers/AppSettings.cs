namespace WebApi.Helpers;

public class AppSettings
{
    public string Secret { get; set; }
    // refresh token time to live (in days), inactive tokens are
    // automatically deleted from the database after this time
    // ReSharper disable once InconsistentNaming
    public int RefreshTokenTTL { get; set; }
}