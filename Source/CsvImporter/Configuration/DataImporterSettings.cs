// ----------------------------------------------------------------------------
// <copyright file="DataImporterSettings.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Configuration
{
    /// <summary>
    /// Entity that contains the settings for the data importer.
    /// </summary>
    public class DataImporterSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the block size to split the request.
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Gets or sets the file's URI.
        /// </summary>
        public string FileURI { get; set; }

        #endregion Properties
    }
}