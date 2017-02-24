using System;
using System.IO;
using AutoMapper;
using MicroSB.Server.Models;
using MicroSB.Server.Models.Shops;
using MicroSB.Server.ViewModels.Shops;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;

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

			// Add EntityFramework's Identity support.
			services.AddEntityFramework();
			// Add ApplicationDbContext.
			// options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"])
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Filename=./microsb.db"));

			// Add ApplicationDbContext's DbSeeder
			services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();

			// AutoMapper binding configuration
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Shop, ShopViewModel>();
				cfg.CreateMap<ShopViewModel, Shop>();
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ILoggerFactory loggerFactory,
			IDatabaseInitializer databaseInitializer)
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

			// TODO: auth0
			var options = new JwtBearerOptions
			{
				Audience = Configuration["Auth0:ApiIdentifier"],
				Authority = $"https://{Configuration["Auth0:Domain"]}/"
			};
			app.UseJwtBearerAuthentication(options);

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
			}
			catch (AggregateException e)
			{
				throw new Exception(e.ToString());
			}
		}
	}
}
