// ----------------------------------------------------------------------------
// <copyright file="ElementSqlServerSqlClientRepository.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Repository.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;

    using CsvImporter.Configuration;
    using CsvImporter.Database;
    using CsvImporter.Domain;

    using Dapper;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Defines the element implementation for the file importer repository.
    /// </summary>
    public class ElementSqlServerSqlClientRepository : IElementRepository
    {
        #region Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The settings of this section.
        /// </summary>
        private readonly ElementSqlServerSqlClientRepositorySettings settings;

        /// <summary>
        /// The sql server settings.
        /// </summary>
        private readonly SqlServerSettings sqlServerSettings;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementSqlServerSqlClientRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="sqlServerSettings">The sql server settings.</param>
        /// <param name="settings">The settings of this section.</param>
        public ElementSqlServerSqlClientRepository(ILogger<ElementSqlServerDapperRepository> logger, SqlServerSettings sqlServerSettings, ElementSqlServerSqlClientRepositorySettings settings)
        {
            this.logger = logger;
            this.sqlServerSettings = sqlServerSettings;
            this.settings = settings;
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

        /// <summary>
        /// Saves the element collection into the repository.
        /// </summary>
        /// <param name="elements">The element's collection to save.</param>
        /// <returns><b>True</b> if the element's collection is saved successfully, <b>False</b> in otherwise.</returns>
        public async Task SaveCollection(List<Element> elements)
        {
            ListDataReader<Element> reader = new ListDataReader<Element>(elements);
            using (var sqlConnection = new SqlConnection(this.sqlServerSettings.ConnectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        using (var sqlBulk = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.TableLock, transaction))
                        {
                            sqlBulk.DestinationTableName = "[dbo].[Element]";
                            await sqlBulk.WriteToServerAsync(reader);
                            sqlBulk.BatchSize = this.settings.BatchSize;
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError(ex, "Error al grabar los registros en base de datos");
                        transaction.Rollback();
                    }
                }
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