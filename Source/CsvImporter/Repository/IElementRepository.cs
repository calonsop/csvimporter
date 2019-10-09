// ----------------------------------------------------------------------------
// <copyright file="IElementRepository.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CsvImporter.Domain;

    /// <summary>
    /// Contract for elements repository.
    /// </summary>
    public interface IElementRepository
    {
        #region Methods

        /// <summary>
        /// Initializes the repository.
        /// </summary>
        /// <returns>The resulted task.</returns>
        Task Initialize();

        /// <summary>
        /// Saves the element collection into the repository.
        /// </summary>
        /// <param name="elements">The element's collection to save.</param>
        /// <returns><b>True</b> if the element's collection is saved successfully, <b>False</b> in otherwise.</returns>
        Task SaveCollection(List<Element> elements);

        #endregion Methods
    }
}