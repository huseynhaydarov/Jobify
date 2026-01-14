namespace Jobify.WebAPI.Configurations;

public class MassTransitConfiguration
{
    public const string Key = "MassTransit";

    public string Url { get; set; }
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
