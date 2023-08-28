using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Chat.Client;
using Chat.Client.Managers;
using MudBlazor.Services;
using MudBlazor;

namespace Chat.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("Chat.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
        // Supply HttpClient instances that include access tokens when making requests to the server project
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Chat.ServerAPI"));

        builder.Services.AddApiAuthorization();
        builder.Services.AddTransient<IChatManager, ChatManager>();
        await builder.Build().RunAsync();
    }
}

