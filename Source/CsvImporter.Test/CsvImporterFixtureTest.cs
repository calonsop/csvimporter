// ----------------------------------------------------------------------------
// <copyright file="CsvImporterFixtureTest.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Test
{
    using System;
    using System.Collections.Generic;

    using CsvImporter.Domain;
    using CsvImporter.Repository;
    using CsvImporter.Test.Repository.Stubs;

    using Microsoft.Extensions.DependencyInjection;

    using Moq;

    /// <summary>
    /// Defines the fixtures setup of the tests.
    /// </summary>
    public class CsvImporterFixtureTest
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvImporterFixtureTest"/> class.
        /// </summary>
        public CsvImporterFixtureTest()
        {
            var serviceCollection = new ServiceCollection();

            this.SetupMoqFileImporterRepository(serviceCollection);
            this.SetupMoqElementRepository(serviceCollection);

            this.ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the moq of elementRepository.
        /// </summary>
        public Mock<IElementRepository> MoqElementRepository { get; private set; } = new Mock<IElementRepository>();

        /// <summary>
        /// Gets the moq of fileImporterRepository.
        /// </summary>
        public Mock<IFileImporterRepository> MoqFileImporterRepository { get; private set; } = new Mock<IFileImporterRepository>();

        /// <summary>
        /// Gets ServiceProvider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates a mock implementation for IElementRepository.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        private void SetupMoqElementRepository(ServiceCollection serviceCollection)
        {
            // Initialize
            this.MoqElementRepository.Setup(a => a.Initialize());

            // SaveCollection
            this.MoqElementRepository.Setup(a => a.SaveCollection(It.IsAny<List<Element>>()));

            serviceCollection.AddSingleton<IElementRepository>((a) => this.MoqElementRepository.Object);
        }

        /// <summary>
        /// Generates a mock implementation for IFileImporterRepository.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        private void SetupMoqFileImporterRepository(ServiceCollection serviceCollection)
        {
            ////  GetFileSize
            ////moqFileImporterRepository.Setup(a => a.GetFileSize(It.IsAny<string>())).Returns(async () => await Task.Run(() => 400));
            ////moqFileImporterRepository.Setup(a => a.GetFileSize(It.Is<string>(c => c == "a"))).Returns(async () => await Task.Run(() => 100));
            ////moqFileImporterRepository.Setup(a => a.GetFileSize(It.Is<string>(c => c == "b"))).Returns(async () => await Task.Run(() => 200));

            serviceCollection.AddSingleton<IFileImporterRepository>((a) => new FileImportRepositoryStub());
        }

        #endregion Methods
    }
}