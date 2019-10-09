// ----------------------------------------------------------------------------
// <copyright file="IDataImporter.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.ApplicationServices
{
    using System.Threading.Tasks;

    /// <summary>
    /// The data importer contract.
    /// </summary>
    public interface IDataImporter
    {
        #region Methods

        /// <summary>
        /// The execution method.
        /// </summary>
        /// <returns>The resulted task.</returns>
        Task Run();

        #endregion Methods
    }
}