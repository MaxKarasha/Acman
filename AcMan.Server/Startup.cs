using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Integration.Converter;
using AcMan.Server.Integration.OData;
using AcMan.Server.Integration.RemoteRepository;
using AcMan.Server.Integration.SyncStrategy;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace AcMan.Server
{
	public class Startup
	{
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IHostingEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath + "/Properties")
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
		{
			services.AddCors(c =>
			{
				c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
			});
			//services.AddDbContext<AcManContext>(opts => opts.UseNpgsql(Configuration.GetConnectionString("PostgreSql")));
			//services.AddDbContext<AcManContext>(opts => opts.UseInMemoryDatabase("AcMan"));
			services.AddDbContext<AcManContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("SqlServer"), providerOptions => providerOptions.CommandTimeout(60)));

			services.AddTransient<AccountRepository>();
            services.AddTransient<ActivityAdditionalInfoRepository>();
            services.AddTransient<ActivityRepository>();
            services.AddTransient<EndSystemRepository>();
            services.AddTransient<ProjectRepository>();
            services.AddTransient<TagRepository>();
            services.AddTransient<UserInSystemRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<SynchronizationRepository>();
            services.AddTransient<ActivityBpmOdataRepository>();
            services.AddTransient<BpmOdataConverter>();            
            services.AddSingleton(
                new ODBase(
                    Configuration.GetSection("Bpmonline")["Url"],
                    Configuration.GetSection("Bpmonline")["Login"],
                    Configuration.GetSection("Bpmonline")["Password"]
                )
            );

            services.AddTransient<ISyncStrategy, BpmonlineSyncStrategy>();
            services.AddTransient<IKeyReader, QueryStringKeyReader>();

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
            app.UseCurrentConnection();
            app.UseSwagger();
			app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMvc();
        }
	}
}
