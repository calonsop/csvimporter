// ----------------------------------------------------------------------------
// <copyright file="FileImportRepositoryStub.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Test.Repository.Stubs
{
    using System.IO;
    using System.Threading.Tasks;

    using CsvImporter.Repository;
    using CsvImporter.Test.Properties;

    /// <summary>
    /// Defines a FileImportRepository stub implementation.
    /// In this implementation are used files in stored in a resource file.
    /// </summary>
    public class FileImportRepositoryStub : IFileImporterRepository
    {
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
            await Task.Yield();
            string result;
            try
            {
                string filecontent = this.GetFileContent(fileURI);
                result = filecontent.Substring(from, length);
            }
            catch
            {
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
            await Task.Yield();
            long result;
            try
            {
                string filecontent = this.GetFileContent(fileURI);
                result = filecontent.Length;
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// Gets the filecontent of a file.
        /// </summary>
        /// <param name="fileURI">The filenaame.</param>
        /// <returns>The content of the file.</returns>
        private string GetFileContent(string fileURI)
        {
            string filecontent;
            switch (fileURI)
            {
                case nameof(FileResources.FileFormatError1):
                    filecontent = FileResources.FileFormatError1;
                    break;
                case nameof(FileResources.FileOk1):
                    filecontent = FileResources.FileOk1;
                    break;
                case nameof(FileResources.FileOk2):
                    filecontent = FileResources.FileOk2;
                    break;
                default:
                    throw new FileNotFoundException("Fichero no encontrado", fileURI);
            }

            return filecontent;
        }

        #endregion Methods
    }
}