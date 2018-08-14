namespace PictIt
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using PictIt.Models;
    using PictIt.Services;

    using ScottBrady91.AspNetCore.Identity;

    using SendGrid;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                // Configure the context to use Azure Microsoft SQL Server.
                options.UseSqlServer(_configuration.GetConnectionString("AzureConnection")).UseLazyLoadingProxies();
            });

            #region Identity Config

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddUserManager<CustomUserManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;

                // SignIn Options
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            /* Password hashing config
            /* Available options:
            /* Interactive for interactive sessions (fast: uses 32MB of RAM).
            /* Moderate for normal use (moderate: uses 128MB of RAM).
            /* Sensitive for highly sensitive data (slow: uses 512MB of RAM).
            */
            services.AddScoped<IPasswordHasher<User>, Argon2PasswordHasher<User>>();
            services.Configure<Argon2PasswordHasherOptions>(options => options.Strength = Argon2HashStrength.Sensitive);

            services.AddIdentityServer(
            options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.UserInteraction.LoginUrl = "/login";
            })
            .AddAspNetIdentity<User>()
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(Config.GetIdentityResources())
            .AddInMemoryApiResources(Config.GetApis())
            .AddInMemoryClients(Config.GetClients());

            #endregion

            #region Authentication Config

            services.AddAuthentication()
                //.AddDeviantArt(
                //    options =>
                //    {

                //    })
                .AddFacebook(
                    options =>
                    {
                        options.AppId = _configuration["Facebook:AppId"];
                        options.AppSecret = _configuration["Facebook:AppSecret"];
                    })
                .AddGitHub(
                    options =>
                    {
                        options.ClientId = _configuration["GitHub:ClientId"];
                        options.ClientSecret = _configuration["GitHub:ClientSecret"];
                    })
                .AddGoogle(
                    options =>
                    {
                        options.ClientId = _configuration["Google:ClientId"];
                        options.ClientSecret = _configuration["Google:ClientSecret"];
                    })
                .AddLinkedIn(
                    options =>
                    {
                        options.ClientId = _configuration["LinkedIn:ClientId"];
                        options.ClientSecret = _configuration["LinkedIn:ClientSecret"];
                    })
                .AddMicrosoftAccount(
                    options =>
                    {
                        options.ClientId = _configuration["Microsoft:AppId"];
                        options.ClientSecret = _configuration["Microsoft:AppSecret"];
                    })
                /* Temporary unavailable
                .AddSoundCloud(
                    options =>
                        {

                        })
                */
                .AddTwitter(
                    options =>
                    {
                        options.ConsumerKey = _configuration["Twitter:ApiKey"];
                        options.ConsumerSecret = _configuration["Twitter:ApiSecret"];
                    });

            #endregion

            #region Authorization Config

            services.AddAuthorization();

            #endregion

            // Email service
            services.AddSingleton<IEmailSender, EmailSender>();
            services.Configure<SendGridClientOptions>(_configuration.GetSection("SendGrid"));

            // Enforce SSL
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
                options.RequireHttpsPermanent = true;

                if (_env.IsDevelopment())
                    options.SslPort = 44399;
            });

            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/not-found");
                app.UseHsts();
            }
         
            app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowCredentials();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });

            app.UseIdentityServer();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                // Unavailable for now due to BootModuleBuilder never calling back
                // More info at https://github.com/aspnet/Templating/issues/593
                // spa.UseSpaPrerendering(options =>
                // {
                //    options.BootModulePath = $"{spa.Options.SourcePath}/dist/server/main.js";
                //    options.BootModuleBuilder = env.IsDevelopment()
                //                                  ? new AngularCliBuilder(npmScript: "build:ssr")
                //                                  : null;

                ////    options.ExcludeUrls = new[] { "/sockjs-node" };

                ////   /* This is used to supply data to main.server.ts
                //    options.SupplyData = (context, data) =>
                //    {
                //        // Creates a new value called isHttpsRequest that's passed to TypeScript code
                //        data["isHttpsRequest"] = context.Request.IsHttps;
                //    };
                //    */
                // });
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
