// ----------------------------------------------------------------------------
// <copyright file="Program.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    using CsvImporter.ApplicationServices;
    using CsvImporter.ApplicationServices.Imp;
    using CsvImporter.Configuration;
    using CsvImporter.Repository;
    using CsvImporter.Repository.Imp;

    using global::Serilog;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The main class.
    /// </summary>
    public class Program
    {
        #region Methods

        /// <summary>
        /// The application entrypoint.
        /// </summary>
        /// <returns>The resulted task.</returns>
        public static async Task Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es-ES");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es-ES");

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            var services = new ServiceCollection();

            ConfigureServices(services, configuration);

            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<Program>>();
            logger.LogInformation("IOC Container init.");
            var dataImporter = serviceProvider.GetService<IDataImporter>();

            await dataImporter.Run().ConfigureAwait(false);
        }

        /// <summary>
        /// The services configurator.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="configuration">The configuration.</param>
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(configure => configure.AddSerilog())
                    .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            services.AddTransient<IDataImporter, DataImporter>();
            services.AddSingleton<IFileImporterRepository, FileImporterBlobStorageRepository>();
            services.AddSingleton<IElementRepository, ElementSqlServerSqlClientRepository>();
            services.AddSingleton<DataImporterSettings>(configuration.GetSection(nameof(DataImporterSettings)).Get<DataImporterSettings>());
            services.AddSingleton<SqlServerSettings>(configuration.GetSection(nameof(SqlServerSettings)).Get<SqlServerSettings>());
            services.AddSingleton<ElementSqlServerSqlClientRepositorySettings>(configuration.GetSection(nameof(ElementSqlServerSqlClientRepositorySettings)).Get<ElementSqlServerSqlClientRepositorySettings>());
        }

        #endregion Methods
    }
}