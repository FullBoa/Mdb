using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Mdb
{
    /// <summary>
    /// Returns configuration options for swagger.
    /// </summary>
    public class SwaggerOptionFactory
    {
        public const string AppDocumentationFilename = "Mdb.xml";

        /// <summary>
        /// Returns options for swagger doc generator.
        /// </summary>
        /// <param name="descriptionProvider"><see cref="IApiVersionDescriptionProvider"/></param>
        /// <returns>Options for swagger doc generator <see cref="SwaggerGenOptions"/></returns>
        public static Action<SwaggerGenOptions> GetConfigurationOptions(
            IApiVersionDescriptionProvider descriptionProvider)
        {
            return options =>
            {
                foreach (var description in descriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        new Info()
                        {
                            Title = $"Mdb v{description.ApiVersion}",
                            Version = description.ApiVersion.ToString()
                        });
                }

                var filePath = Path.Combine(AppContext.BaseDirectory, AppDocumentationFilename);
                options.IncludeXmlComments(filePath);
                options.DescribeAllEnumsAsStrings();
            };
        }

        /// <summary>
        /// Returns options for swagger UI.
        /// </summary>
        /// <param name="descriptionProvider"><see cref="IApiVersionDescriptionProvider"/></param>
        /// <returns>Options for swagger UI <see cref="SwaggerUIOptions"/></returns>
        public static Action<SwaggerUIOptions> GetUiConfigurationOptions(
            IApiVersionDescriptionProvider descriptionProvider)
        {
            return options =>
            {
                foreach (var description in descriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }
            };
        }
    }
}