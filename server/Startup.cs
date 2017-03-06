using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MicroSB.Server.Models;
using MicroSB.Server.Models.Shops;
using MicroSB.Server.Models.Users;
using MicroSB.Server.Services;
using MicroSB.Server.ViewModels.Shops;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using OpenIddict.Core;
using OpenIddict.Models;

namespace MicroSB.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services
                .AddMvc()
                .AddJsonOptions(opts =>
                {
                    // Force Camel Case to JSON
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            // Add ApplicationDbContext.
            var connection = Configuration.GetConnectionString("microsb");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(connection);

                // Register the entity sets needed by OpenIddict.
                // Note: use the generic overload if you need
                // to replace the default OpenIddict entities.
                options.UseOpenIddict();
            });

            // Register the Identity services.
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Register the OpenIddict services.
            services.AddOpenIddict()
                // Register the Entity Framework stores.
                .AddEntityFrameworkCoreStores<ApplicationDbContext>()

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                .AddMvcBinders()

                // Enable the authorization, logout, token and userinfo endpoints.
                .EnableAuthorizationEndpoint("/connect/authorize")
                .EnableLogoutEndpoint("/connect/logout")
                .EnableTokenEndpoint("/connect/token")
                .EnableUserinfoEndpoint("/api/userinfo")

                // Note: the Mvc.Client sample only uses the authorization code flow but you can enable
                // the other flows if you need to support implicit, password or client credentials.
                .AllowAuthorizationCodeFlow()

                // When request caching is enabled, authorization and logout requests
                // are stored in the distributed cache by OpenIddict and the user agent
                // is redirected to the same page with a single parameter (request_id).
                // This allows flowing large OpenID Connect requests even when using
                // an external authentication provider like Google, Facebook or Twitter.
                .EnableRequestCaching()

                // During development, you can disable the HTTPS requirement.
                .DisableHttpsRequirement();

            services.AddTransient<IEmailSender, AuthMessageSender>();
			services.AddTransient<ISmsSender, AuthMessageSender>();

			// Add ApplicationDbContext's DbSeeder
            services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDatabaseInitializer databaseInitializer)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseStaticFiles();
            //prod build includes deps
            if (env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "../client/node_modules/")),
                    RequestPath = "/node_modules"
                });
            }

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), branch =>
            {
                // Add a middleware used to validate access
                // tokens and protect the API endpoints.
                branch.UseOAuthValidation();

                // Alternatively, you can also use the introspection middleware.
                // Using it is recommended if your resource server is in a
                // different application/separated from the authorization server.
                //
                // branch.UseOAuthIntrospection(options =>
                // {
                //     options.Authority = new Uri("http://localhost:54540/");
                //     options.Audiences.Add("resource_server");
                //     options.ClientId = "resource_server";
                //     options.ClientSecret = "875sqd4s5d748z78z7ds1ff8zz8814ff88ed8ea4z4zzd";
                //     options.RequireHttpsMetadata = false;
                // });
            });

            app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), branch =>
			{
				branch.UseStatusCodePagesWithReExecute("/error");

				branch.UseIdentity();

				//branch.UseGoogleAuthentication(new GoogleOptions
				//{
				//	ClientId = "560027070069-37ldt4kfuohhu3m495hk2j4pjp92d382.apps.googleusercontent.com",
				//	ClientSecret = "n2Q-GEw9RQjzcRbU3qhfTj8f"
				//});

				//branch.UseTwitterAuthentication(new TwitterOptions
				//{
				//	ConsumerKey = "6XaCTaLbMqfj6ww3zvZ5g",
				//	ConsumerSecret = "Il2eFzGIrYhz6BWjYhVXBPQSfZuS4xoHpSSyD9PI"
				//});
			});

            app.UseOpenIddict();

            // Add MVC to the request pipeline.
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapSpaFallbackRoute("spa-fallback", new { controller = "Home", action = "Index" });
            });

            // Seed the Database (if needed)
            try
            {
                databaseInitializer.SeedAsync().GetAwaiter().GetResult();

                // Seed the database with the sample applications.
                // Note: in a real world application, this step should be part of a setup script.
                InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
            }
            catch (AggregateException e)
            {
                throw new Exception(e.ToString());
            }

            // AutoMapper binding configuration
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Shop, ShopViewModel>();
                cfg.CreateMap<ShopViewModel, Shop>();
            });
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("mvc", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "mvc",
                        DisplayName = "MVC client application",
                        LogoutRedirectUri = "http://localhost:53507/",
                        RedirectUri = "http://localhost:53507/signin-oidc"
                    };

                    await manager.CreateAsync(application, "901564A5-E7FE-42CB-B10D-61EF6A8F3654", cancellationToken);
                }

                // To test this sample with Postman, use the following settings:
                //
                // * Authorization URL: http://localhost:54540/connect/authorize
                // * Access token URL: http://localhost:54540/connect/token
                // * Client ID: postman
                // * Client secret: [blank] (not used with public clients)
                // * Scope: openid email profile roles
                // * Grant type: authorization code
                // * Request access token locally: yes
                if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var application = new OpenIddictApplication
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUri = "https://www.getpostman.com/oauth2/callback"
                    };

                    await manager.CreateAsync(application, cancellationToken);
                }
            }
        }
    }
}
