// ----------------------------------------------------------------------------
// <copyright file="FileImporterBlobStorageRepository.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Repository.Imp
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    /// <summary>
    /// Defines the blob storage implementation for the file importer repository.
    /// </summary>
    public class FileImporterBlobStorageRepository : IFileImporterRepository
    {
        #region Fields

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileImporterBlobStorageRepository"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FileImporterBlobStorageRepository(ILogger<FileImporterBlobStorageRepository> logger)
        {
            this.logger = logger;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Obtains a part of the file.
        /// </summary>
        /// <param name="fileURI">The file URI.</param>
        /// <param name="from">The cursor init.</param>
        /// <param name="length">The length of the data.</param>
        /// <returns>The content of the block of information.</returns>
        public async Task<string> GetFileBlock(string fileURI, int from, int length)
        {
            string result = string.Empty;
            try
            {
                CloudBlob blob = new CloudBlockBlob(new Uri(fileURI));

                MemoryStream ms = null;
                try
                {
                    ms = new MemoryStream();

                    var accessCondition = new AccessCondition();
                    var blobRequestOptions = new BlobRequestOptions(); //// configurar.
                    var operationContext = new OperationContext();

                    await blob.DownloadRangeToStreamAsync(ms, from, length, accessCondition, blobRequestOptions, operationContext).ConfigureAwait(false);
                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader sr = new StreamReader(ms, true))
                    {
                        ms = null;
                        result = sr.ReadToEnd();
                    }
                }
                finally
                {
                    if (ms != null)
                    {
                        ms.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error al obtener el trozo del fichero ({fileURI},{from},{length})");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Obtains the size of the file.
        /// </summary>
        /// <param name="fileURI">The file URI.</param>
        /// <returns>The size of the file.</returns>
        public async Task<long> GetFileSize(string fileURI)
        {
            long result = 0;
            try
            {
                CloudBlob blob = new CloudBlockBlob(new Uri(fileURI));

                if (await blob.ExistsAsync().ConfigureAwait(false))
                {
                    await blob.FetchAttributesAsync().ConfigureAwait(false);
                    result = blob.Properties.Length;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error al obtener el tamaño del fichero  ({fileURI})");
                throw;
            }

            return result;
        }

        #endregion Methods
    }
}