namespace PictIt
{
    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            // Enforce SSL
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
                options.RequireHttpsPermanent = true;

                if (_env.IsDevelopment())
                    options.SslPort = 44399;
            });


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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                // Unavailable for now
                // More info at https://github.com/aspnet/Templating/issues/593
                //spa.UseSpaPrerendering(options =>
                //{
                //    options.BootModulePath = $"{spa.Options.SourcePath}/dist/server/main.js";
                //    options.BootModuleBuilder = env.IsDevelopment()
                //                                    ? new AngularCliBuilder(npmScript: "build:ssr")
                //                                    : null;
                    
                //    options.ExcludeUrls = new[] { "/sockjs-node" };

                //    /* This is used to supply data to main.server.ts
                //    options.SupplyData = (context, data) =>
                //    {
                //        // Creates a new value called isHttpsRequest that's passed to TypeScript code
                //        data["isHttpsRequest"] = context.Request.IsHttps;
                //    };
                //    */
                //});

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
