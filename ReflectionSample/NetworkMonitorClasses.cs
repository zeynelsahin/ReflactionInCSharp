﻿namespace ReflectionSample;

public class MailService
{
    public void SendMail(string address, string subject)
    {
        Console.WriteLine($"Sending a warnin mail to {address} with subject {subject}");
    }
}
public class SoundHornService
{
    public void SoundHorn(string volume)
    {
        Console.WriteLine($"Making noise with the volume turned up to {volume}");
    }
}

public class NetworkMonitorSettings
{
    public string WarningService { get; set; }
    public string MethodToExecute { get; set; }
    public Dictionary<string, object> PropertyBag { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
}