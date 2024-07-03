namespace BackendForFrontend.Models.Services
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<ILineMessagingService, LineMessagingService>();
            // 其他服務配置...
        }
    }
}
