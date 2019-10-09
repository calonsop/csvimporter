// ----------------------------------------------------------------------------
// <copyright file="Element.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Domain
{
    using System;

    /// <summary>
    /// The element domain object.
    /// </summary>
    public class Element
    {
        #region Properties

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the point of sale identifier.
        /// </summary>
        public string PointOfSale { get; set; }

        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets de amount of stock available.
        /// </summary>
        public int Stock { get; set; }

        #endregion Properties
    }
}