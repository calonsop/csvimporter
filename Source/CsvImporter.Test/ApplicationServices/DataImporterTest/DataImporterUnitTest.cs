// ----------------------------------------------------------------------------
// <copyright file="DataImporterUnitTest.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.Test.ApplicationServices.DataImporterTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CsvImporter.ApplicationServices;
    using CsvImporter.ApplicationServices.Imp;
    using CsvImporter.Configuration;
    using CsvImporter.Domain;
    using CsvImporter.Repository;
    using CsvImporter.Test.Properties;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Moq;

    using Xunit;

    /// <summary>
    /// Defines the test class of the DataImporter application service.
    /// </summary>
    public class DataImporterUnitTest : IClassFixture<CsvImporterFixtureTest>
    {
        #region Fields

        /// <summary>
        /// The fixture.
        /// </summary>
        private readonly CsvImporterFixtureTest fixture;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataImporterUnitTest"/> class.
        /// </summary>
        /// <param name="fixtureClass">The fixture class.</param>
        public DataImporterUnitTest(CsvImporterFixtureTest fixtureClass)
        {
            this.fixture = fixtureClass;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Verifies that with a file with bad format, the method throws an exception.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0039:Use local function", Justification = "Reviewed")]
        public async Task Run_FileFormatError1()
        {
            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 100,
                FileURI = nameof(FileResources.FileFormatError1),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            Func<Task> actTodo = () => sut.Run();

            // ASSERT
            await Assert.ThrowsAsync<FormatException>(actTodo);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        /// <summary>
        /// Verifies the execution with the fileok1 and blocksize to 100.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        public async Task Run_FileOK1_BlockSize100()
        {
            await Task.Yield();

            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 100,
                FileURI = nameof(FileResources.FileOk1),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            await sut.Run();

            // ASSERT
            Assert.Equal(28793, contador);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        /// <summary>
        /// Verifies the execution with the fileok2 and blocksize to 1000.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        public async Task Run_FileOK2_BlockSize1000()
        {
            await Task.Yield();

            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 1000,
                FileURI = nameof(FileResources.FileOk2),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            await sut.Run();

            // ASSERT
            Assert.Equal(13099, contador);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        /// <summary>
        /// Verifies the execution with the fileok2 and blocksize to 10000.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        public async Task Run_FileOK2_BlockSize10000()
        {
            await Task.Yield();

            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 10000,
                FileURI = nameof(FileResources.FileOk2),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            await sut.Run();

            // ASSERT
            Assert.Equal(13099, contador);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        /// <summary>
        /// Verifies the execution with the fileok2 and blocksize to 20000.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        public async Task Run_FileOK2_BlockSize20000()
        {
            await Task.Yield();

            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 20000,
                FileURI = nameof(FileResources.FileOk2),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            await sut.Run();

            // ASSERT
            Assert.Equal(13099, contador);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        /// <summary>
        /// Verifies the execution with the fileok2 and blocksize to 5000.
        /// </summary>
        /// <returns>The resulted task.</returns>
        [Fact]
        public async Task Run_FileOK2_BlockSize5000()
        {
            await Task.Yield();

            // ARRANGE
            this.fixture.MoqElementRepository.Invocations.Clear();
            DataImporterSettings settings = new DataImporterSettings()
            {
                BlockSize = 5000,
                FileURI = nameof(FileResources.FileOk2),
            };

            int contador = 0;

            this.fixture.MoqElementRepository
                .Setup(a => a.SaveCollection(It.IsAny<List<Element>>()))
                .Callback((List<Element> lst) => contador += lst.Count);

            DataImporter sut = new DataImporter(
                new Mock<ILogger<DataImporter>>().Object,
                this.fixture.ServiceProvider.GetRequiredService<IFileImporterRepository>(),
                this.fixture.ServiceProvider.GetRequiredService<IElementRepository>(),
                settings);

            // ACT
            await sut.Run();

            // ASSERT
            Assert.Equal(13099, contador);
            this.fixture.MoqElementRepository.Verify(a => a.Initialize(), Times.Once());
        }

        #endregion Methods
    }
}