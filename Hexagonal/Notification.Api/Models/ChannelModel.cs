namespace Notification.Blazor.Models;

public class ChannelModel
{
    public string Name { get; set; } = string.Empty;
    public string Adapter { get; set; } = string.Empty;
    public string UseCase { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
}
