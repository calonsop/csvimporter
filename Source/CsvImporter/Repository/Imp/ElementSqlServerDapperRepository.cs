// ----------------------------------------------------------------------------
// <copyright file="ElementSqlServerDapperRepository.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Repository.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CsvImporter.Configuration;
    using CsvImporter.Domain;

    using Dapper;

    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Defines the element implementation for the file importer repository.
    /// </summary>
    public class ElementSqlServerDapperRepository : IElementRepository
    {
        #region Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The sql server settings.
        /// </summary>
        private readonly SqlServerSettings sqlServerSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSqlServerDapperRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sqlServerSettings">The sql server settings.</param>
        public ElementSqlServerDapperRepository(ILogger<ElementSqlServerDapperRepository> logger, SqlServerSettings sqlServerSettings)
        {
            this.logger = logger;
            this.sqlServerSettings = sqlServerSettings;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes the repository.
        /// </summary>
        /// <returns>The resulted task.</returns>
        public async Task Initialize()
        {
            try
            {
                this.logger.LogInformation("Inicializando el repositorio de elementos");
                using (var connection = new SqlConnection(this.sqlServerSettings.ConnectionString))
                {
                    connection.Open();

                    this.logger.LogInformation("Comprobando la existencia de la tabla.");
                    if (!await this.ExistsTable(connection).ConfigureAwait(false))
                    {
                        this.logger.LogInformation("Puesto que no existe se crea.");
                        await this.CreateTable(connection).ConfigureAwait(false);
                        this.logger.LogInformation("Tabla creada.");
                    }
                    else
                    {
                        this.logger.LogInformation("Puesto que existe se vacia.");
                        await this.ResetTable(connection).ConfigureAwait(false);
                        this.logger.LogInformation("Tabla vaciada.");
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al inicializar el repositorio");
                throw;
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        /// <summary>
        /// Saves the element collection into the repository.
        /// </summary>
        /// <param name="elements">The element's collection to save.</param>
        /// <returns><b>True</b> if the element's collection is saved successfully, <b>False</b> in otherwise.</returns>
        public async Task SaveCollection(List<Element> elements)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                this.logger.LogInformation("Inicializando el repositorio de elementos");
                while (elements.Count > 0)
                {
                    int elementosTratar = Math.Min(1000, elements.Count);
                    string query = this.GenerateBuklInsertQuery(elements.Take(elementosTratar));
                    elements.RemoveRange(0, elementosTratar);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    this.ExecuteQuery(query).ConfigureAwait(false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al guardar la colección.");
                throw;
            }
        }

        /// <summary>
        /// Creates the Element table in database.
        /// </summary>
        /// <param name="conn">The database connection.</param>
        /// <returns>The resulted task.</returns>
        private async Task CreateTable(SqlConnection conn)
        {
            var query = @"CREATE TABLE [dbo].[Element]
                                (
                                    [Id] INT NOT NULL PRIMARY KEY Identity,
                                    [PointOfSale] varchar(100) not null,
                                    [Product] varchar(100) not null,
                                    [Date] datetime not null,
                                    [Stock] int not null
                                )";

            await conn.ExecuteAsync(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Serializes an element in Sql Insert format to use in a bulk query.
        /// </summary>
        /// <param name="src">The element to serialize.</param>
        /// <returns>The serialization result.</returns>
        private string ElementToSQLServerInsertFormat(Element src)
        {
            return $"('{src.PointOfSale}','{src.Product}','{src.Date.ToString("yyyyMMdd")}',{src.Stock})";
        }

        /// <summary>
        /// Executes a query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The resulted task.</returns>
        private async Task ExecuteQuery(string query)
        {
            using (var connection = new SqlConnection(this.sqlServerSettings.ConnectionString))
            {
                connection.Open();
                ////var transation = connection.BeginTransaction();
                await connection.ExecuteAsync(query).ConfigureAwait(false);
                ////transation.Commit();
            }
        }

        /// <summary>
        /// Determines if the Element table exists in database.
        /// </summary>
        /// <param name="conn">The database connection.</param>
        /// <returns><b>True</b> if the table exists in database, <b>False</b> in otherwise.</returns>
        private async Task<bool> ExistsTable(SqlConnection conn)
        {
            var result = await conn.QueryAsync<object>("SELECT* FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo'  AND TABLE_NAME = 'Element'").ConfigureAwait(false);
            return result.AsList().Any();
        }

        /// <summary>
        /// Generates a bulk insertion commmand.
        /// </summary>
        /// <param name="src">Elements to insert.</param>
        /// <returns>The bulk command with the insert operation.</returns>
        private string GenerateBuklInsertQuery(IEnumerable<Element> src)
        {
            IEnumerable<string> elementos = src.Select(a => this.ElementToSQLServerInsertFormat(a));
            string strElements = string.Join(",", elementos);
            string query = $"INSERT INTO [dbo].[Element] ([PointOfSale], [Product], [Date],[Stock]) VALUES {strElements} ;";
            return query;
        }

        /// <summary>
        /// Resets the Element table in database.
        /// </summary>
        /// <param name="conn">The database connection.</param>
        /// <returns><b>True</b> if the table was reset in database succesfully, <b>False</b> in otherwise.</returns>
        private async Task ResetTable(SqlConnection conn)
        {
            var query = @"TRUNCATE TABLE [dbo].[Element]";

            await conn.ExecuteAsync(query).ConfigureAwait(false);
        }

        #endregion Methods
    }
}