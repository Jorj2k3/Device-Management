namespace DeviceManagement.Api.Services
{
    public interface IAiService
    {
        Task<string> GenerateDeviceDescriptionAsync(string name, string manufacturer, string os, string type, int ram, string processor);
    }
}
