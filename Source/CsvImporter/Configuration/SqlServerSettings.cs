// ----------------------------------------------------------------------------
// <copyright file="SqlServerSettings.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Configuration
{
    /// <summary>
    /// Entity that contains the settings for sql server connection.
    /// </summary>
    public class SqlServerSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the blob storage account connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        #endregion Properties
    }
}