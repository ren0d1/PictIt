namespace PictIt
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

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

                    var keyVaultConfigBuilder = new ConfigurationBuilder();

                    keyVaultConfigBuilder.AddAzureKeyVault(
                        $"https://{builtConfig.GetSection("AzureKeyVault")["VaultName"]}.vault.azure.net/",
                        builtConfig.GetSection("AzureKeyVault")["ClientId"],
                        builtConfig.GetSection("AzureKeyVault")["ClientSecret"]);

                    var keyVaultConfig = keyVaultConfigBuilder.Build();

                    config.AddConfiguration(keyVaultConfig);
                })
                .UseStartup<Startup>();
    }
}
