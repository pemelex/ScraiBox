using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ScraiBox.Core;

namespace ScraiBox.Gui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().UseMauiCommunityToolkit();
            builder.Services.AddMauiBlazorWebView();

            // Register your Core tools
            // Default to a placeholder, we will update it via UI
            builder.Services.AddSingleton(new ProjectMapper(AppDomain.CurrentDomain.BaseDirectory));
            builder.Services.AddSingleton(new ContextScryer(AppDomain.CurrentDomain.BaseDirectory));
            builder.Services.AddSingleton<CommandInterceptor>();

            return builder.Build();
        }
    }
}
