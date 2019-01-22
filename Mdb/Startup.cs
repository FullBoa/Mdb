using System;
using Mdb.Dal;
using Mdb.Dal.KvData;
using Mdb.Logic.Stores;
using Mdb.Logic.TextProcessors;
using Mdb.Midlewares.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mdb
{
    public class Startup
    {
        private const string StoreModeItemName = "Persistent";
        private const string DatabaseFilenameItemName = "DataFilename";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonFormatters();
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");

            // Swagger docs
            var descriptionProvider =
                services.BuildServiceProvider()
                    .GetRequiredService<IApiVersionDescriptionProvider>();
            services.AddSwaggerGen(SwaggerOptionFactory.GetConfigurationOptions(descriptionProvider));

            services.AddSingleton<ITextProcessor, CapitalizeTextProcessor>();
            var isPersistent = Configuration.GetValue<bool>(StoreModeItemName);
            if (!isPersistent)
                services.AddSingleton<IStore, InMemoryStore>();
            else
            {
                var databaseFilename = Configuration.GetValue<string>(DatabaseFilenameItemName);
                services.AddSingleton<IDataRepository, DataRepository>(
                    provider => new DataRepository(databaseFilename));
                services.AddSingleton<IStore, PersistentStore>();
            }

            var logger = LoggerFactory.GetLogger();
            services.AddSingleton(logger);

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            IApiVersionDescriptionProvider descriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseRequestLogging();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(SwaggerOptionFactory.GetUiConfigurationOptions(descriptionProvider));
        }
    }
}