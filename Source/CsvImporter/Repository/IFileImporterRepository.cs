// ----------------------------------------------------------------------------
// <copyright file="IFileImporterRepository.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Repository
{
    using System.Threading.Tasks;

    /// <summary>
    /// Contract for file importer's repository.
    /// </summary>
    public interface IFileImporterRepository
    {
        #region Methods

        /// <summary>
        /// Obtains a part of the file.
        /// </summary>
        /// <param name="fileURI">The file URI.</param>
        /// <param name="from">The cursor init.</param>
        /// <param name="length">The length of the data.</param>
        /// <returns>The content of the block of information.</returns>
        Task<string> GetFileBlock(string fileURI, int from, int length);

        /// <summary>
        /// Obtains the size of the file.
        /// </summary>
        /// <param name="fileURI">The file URI.</param>
        /// <returns>The size of the file.</returns>
        Task<long> GetFileSize(string fileURI);

        #endregion Methods
    }
}