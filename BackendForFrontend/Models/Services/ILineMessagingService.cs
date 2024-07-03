namespace BackendForFrontend.Models.Services
{
    public interface ILineMessagingService
    {
        Task SendPushMessageAsync(string userId, string message);

    }
}
