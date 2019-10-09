// ----------------------------------------------------------------------------
// <copyright file="ElementSqlServerSqlClientRepositorySettings.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Configuration
{
    /// <summary>
    /// Entity that contains the settings for element sql server sql client repository.
    /// </summary>
    public class ElementSqlServerSqlClientRepositorySettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the batch size to batch particioning un bulk insert.
        /// </summary>
        public int BatchSize { get; set; }

        #endregion Properties
    }
}