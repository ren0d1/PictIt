namespace PictIt
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.Services.AppAuthentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.AzureKeyVault;

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseIISIntegration()
                .ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();

                    config.AddAzureKeyVault(
                        $"https://{builtConfig.GetSection("AzureKeyVault")["VaultName"]}.vault.azure.net/",
                        builtConfig.GetSection("AzureKeyVault")["ClientId"],
                        builtConfig.GetSection("AzureKeyVault")["ClientSecret"]);
                })
                .UseStartup<Startup>();
    }
}
