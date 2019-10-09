// ----------------------------------------------------------------------------
// <copyright file="DataImporter.cs" company="CristianAlonsoSoft">
//     Copyright © CristianAlonsoSoft. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace CsvImporter.ApplicationServices.Imp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CsvImporter.Configuration;
    using CsvImporter.Domain;
    using CsvImporter.Repository;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The data importer.
    /// </summary>
    public class DataImporter : IDataImporter
    {
        #region Fields

        /// <summary>
        /// The contents that are ready to record.
        /// </summary>
        private readonly List<string> contents;

        /// <summary>
        /// The locker to use the contents collection in async mode.
        /// </summary>
        private readonly object contentsLocker;

        /// <summary>
        /// The section settings.
        /// </summary>
        private readonly DataImporterSettings dataImporterSettings;

        /// <summary>
        /// The element repository.
        /// </summary>
        private readonly IElementRepository elementRepository;

        /// <summary>
        /// The file importer repository.
        /// </summary>
        private readonly IFileImporterRepository fileImporterRepository;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Contains the last line or part of the last line of the last block.
        /// </summary>
        private string ultimaLineaAnterior;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataImporter"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="fileImporterRepository">The file importer repository.</param>
        /// <param name="elementRepository">The element repository.</param>
        /// <param name="dataImporterSettings">The data importer settings.</param>
        public DataImporter(
            ILogger<DataImporter> logger,
            IFileImporterRepository fileImporterRepository,
            IElementRepository elementRepository,
            DataImporterSettings dataImporterSettings)
        {
            this.logger = logger;
            this.fileImporterRepository = fileImporterRepository;
            this.elementRepository = elementRepository;
            this.dataImporterSettings = dataImporterSettings;

            this.contents = new List<string>();
            this.contentsLocker = new object();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// The execution method.
        /// </summary>
        /// <returns>The resulted task.</returns>
        public async Task Run()
        {
            try
            {
                await this.elementRepository.Initialize().ConfigureAwait(false);

                this.logger.LogInformation("Running");

                var size = await this.fileImporterRepository.GetFileSize(this.dataImporterSettings.FileURI).ConfigureAwait(false);

                int numBlocks = (int)(size / this.dataImporterSettings.BlockSize);

                if (size % this.dataImporterSettings.BlockSize > 0)
                {
                    numBlocks++;
                }

                this.ultimaLineaAnterior = string.Empty;
                for (int i = 0; i < numBlocks; i++)
                {
                    string blockContent = await this.fileImporterRepository.GetFileBlock(this.dataImporterSettings.FileURI, i * this.dataImporterSettings.BlockSize, Math.Min(this.dataImporterSettings.BlockSize, (int)(size - (i * this.dataImporterSettings.BlockSize)))).ConfigureAwait(false);
                    await this.PushData(blockContent).ConfigureAwait(false);
                    List<Element> elementsToSave = new List<Element>();
                    lock (this.contentsLocker)
                    {
                        // si es el primer bloque borramos la primera fila que tiene las cabeceras.
                        if (i == 0)
                        {
                            this.contents.RemoveAt(0);
                        }

                        // Si es el ultimo, la fila restante o es una fila completa o no es nada.
                        if (i == numBlocks - 1 && this.IsComplete(this.ultimaLineaAnterior, 4))
                        {
                            elementsToSave.Add(this.ElementFromCSV(this.ultimaLineaAnterior));
                        }

                        elementsToSave.AddRange(this.contents.Select(a => this.ElementFromCSV(a)));
                        this.contents.Clear();
                    }

                    await this.elementRepository.SaveCollection(elementsToSave).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error");
                throw;
            }
        }

        /// <summary>
        /// Converts a csv file line to element.
        /// </summary>
        /// <param name="src">The csv file line.</param>
        /// <returns>The resulted element.</returns>
        private Element ElementFromCSV(string src)
        {
            Element result = new Element();
            try
            {
                string[] fields = src.Split(';');
                result.PointOfSale = fields[0];
                result.Product = fields[1];
                result.Date = DateTime.ParseExact(fields[2], "yyyy-MM-dd", null);
                result.Stock = int.Parse(fields[3]);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error al convertir una fila del csv a un elemento");
                throw;
            }

            return result;
        }

        /// <summary>
        /// Verify if the line is complete.
        /// </summary>
        /// <param name="lineContent">The line content.</param>
        /// <param name="numFieldsExpected">The number of fields expected.</param>
        /// <returns><b>True</b> if the line is complete, <b>False</b> in otherwise.</returns>
        private bool IsComplete(string lineContent, int numFieldsExpected)
        {
            int numFileds = lineContent.Split(";").Length;
            return numFileds == numFieldsExpected;
        }

        /// <summary>
        /// Push the current block to collection to work later.
        /// </summary>
        /// <param name="content">The block content.</param>
        /// <returns>The resulted task.</returns>
        private async Task PushData(string content)
        {
            content = this.ultimaLineaAnterior + content;
            await Task.Yield();
            lock (this.contentsLocker)
            {
                string[] splited = content.Split('\n');
                for (int i = 0; i < splited.Length; i++)
                {
                    if (i == splited.Length - 1)
                    {
                        this.ultimaLineaAnterior = splited[i];
                    }
                    else
                    {
                        this.contents.Add(splited[i].Replace("\r", string.Empty));
                    }
                }
            }
        }

        #endregion Methods
    }
}