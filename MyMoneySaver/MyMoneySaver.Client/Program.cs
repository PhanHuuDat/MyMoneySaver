using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

namespace MyMoneySaver.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Add MudBlazor services
            builder.Services.AddMudServices();

            await builder.Build().RunAsync();
        }
    }
}
